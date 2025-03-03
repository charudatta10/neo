// Copyright (C) 2015-2025 The Neo Project.
//
// OracleRequest.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Extensions;
using Neo.VM;
using Neo.VM.Types;
using Array = Neo.VM.Types.Array;

namespace Neo.SmartContract.Native
{
    /// <summary>
    /// Represents an Oracle request in smart contracts.
    /// </summary>
    public class OracleRequest : IInteroperable
    {
        /// <summary>
        /// The original transaction that sent the related request.
        /// </summary>
        public UInt256 OriginalTxid;

        /// <summary>
        /// The maximum amount of GAS that can be used when executing response callback.
        /// </summary>
        public long GasForResponse;

        /// <summary>
        /// The url of the request.
        /// </summary>
        public string Url;

        /// <summary>
        /// The filter for the response.
        /// </summary>
        public string Filter;

        /// <summary>
        /// The hash of the callback contract.
        /// </summary>
        public UInt160 CallbackContract;

        /// <summary>
        /// The name of the callback method.
        /// </summary>
        public string CallbackMethod;

        /// <summary>
        /// The user-defined object that will be passed to the callback.
        /// </summary>
        public byte[] UserData;

        public void FromStackItem(StackItem stackItem)
        {
            Array array = (Array)stackItem;
            OriginalTxid = new UInt256(array[0].GetSpan());
            GasForResponse = (long)array[1].GetInteger();
            Url = array[2].GetString();
            Filter = array[3].GetString();
            CallbackContract = new UInt160(array[4].GetSpan());
            CallbackMethod = array[5].GetString();
            UserData = array[6].GetSpan().ToArray();
        }

        public StackItem ToStackItem(IReferenceCounter referenceCounter)
        {
            return new Array(referenceCounter)
            {
                OriginalTxid.ToArray(),
                GasForResponse,
                Url,
                Filter ?? StackItem.Null,
                CallbackContract.ToArray(),
                CallbackMethod,
                UserData
            };
        }
    }
}
