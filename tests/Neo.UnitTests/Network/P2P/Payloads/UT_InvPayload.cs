// Copyright (C) 2015-2025 The Neo Project.
//
// UT_InvPayload.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Extensions;
using Neo.Network.P2P.Payloads;
using System;
using System.Linq;

namespace Neo.UnitTests.Network.P2P.Payloads
{
    [TestClass]
    public class UT_InvPayload
    {
        [TestMethod]
        public void Size_Get()
        {
            var test = InvPayload.Create(InventoryType.TX, UInt256.Zero);
            Assert.AreEqual(34, test.Size);

            test = InvPayload.Create(InventoryType.TX, UInt256.Zero, UInt256.Parse("01ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00a4"));
            Assert.AreEqual(66, test.Size);
        }

        [TestMethod]
        public void CreateGroup()
        {
            var hashes = new UInt256[InvPayload.MaxHashesCount + 1];

            for (int x = 0; x < hashes.Length; x++)
            {
                byte[] data = new byte[32];
                Array.Copy(BitConverter.GetBytes(x), data, 4);
                hashes[x] = new UInt256(data);
            }

            var array = InvPayload.CreateGroup(InventoryType.TX, hashes).ToArray();

            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(InventoryType.TX, array[0].Type);
            Assert.AreEqual(InventoryType.TX, array[1].Type);
            CollectionAssert.AreEqual(hashes.Take(InvPayload.MaxHashesCount).ToArray(), array[0].Hashes);
            CollectionAssert.AreEqual(hashes.Skip(InvPayload.MaxHashesCount).ToArray(), array[1].Hashes);
        }

        [TestMethod]
        public void DeserializeAndSerialize()
        {
            var test = InvPayload.Create(InventoryType.TX, UInt256.Zero, UInt256.Parse("01ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00ff00a4"));
            var clone = test.ToArray().AsSerializable<InvPayload>();

            Assert.AreEqual(test.Type, clone.Type);
            CollectionAssert.AreEqual(test.Hashes, clone.Hashes);

            Assert.ThrowsExactly<FormatException>(() => _ = InvPayload.Create((InventoryType)0xff, UInt256.Zero).ToArray().AsSerializable<InvPayload>());
        }
    }
}
