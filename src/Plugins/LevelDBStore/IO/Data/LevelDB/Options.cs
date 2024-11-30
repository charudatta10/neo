// Copyright (C) 2015-2024 The Neo Project.
//
// Options.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;

namespace Neo.IO.Storage.LevelDB
{
    /// <summary>
    /// Options to control the behavior of a database (passed to Open)
    ///
    /// the setter methods for InfoLogger, Env, and Cache only "safe to clean up guarantee". Do not
    /// use Option object if throws.
    /// </summary>
    public class Options : LevelDBHandle
    {
        public static readonly Options Default = new();

        public Options() : base(Native.leveldb_options_create()) { }

        /// <summary>
        /// If true, the database will be created if it is missing.
        /// </summary>
        public bool CreateIfMissing
        {
            set { Native.leveldb_options_set_create_if_missing(Handle, value); }
        }

        /// <summary>
        /// If true, an error is raised if the database already exists.
        /// </summary>
        public bool ErrorIfExists
        {
            set { Native.leveldb_options_set_error_if_exists(Handle, value); }
        }

        /// <summary>
        /// If true, the implementation will do aggressive checking of the
        /// data it is processing and will stop early if it detects any
        /// errors.  This may have unforeseen ramifications: for example, a
        /// corruption of one DB entry may cause a large number of entries to
        /// become unreadable or for the entire DB to become unopenable.
        /// </summary>
        public bool ParanoidChecks
        {
            set { Native.leveldb_options_set_paranoid_checks(Handle, value); }
        }

        // Any internal progress/error information generated by the db will
        // be written to info_log if it is non-NULL, or to a file stored
        // in the same directory as the DB contents if info_log is NULL.

        /// <summary>
        /// Amount of data to build up in memory (backed by an unsorted log
        /// on disk) before converting to a sorted on-disk file.
        ///
        /// Larger values increase performance, especially during bulk loads.
        /// Up to two write buffers may be held in memory at the same time,
        /// so you may wish to adjust this parameter to control memory usage.
        /// Also, a larger write buffer will result in a longer recovery time
        /// the next time the database is opened.
        ///
        /// Default: 4MB
        /// </summary>
        public int WriteBufferSize
        {
            set { Native.leveldb_options_set_write_buffer_size(Handle, (UIntPtr)value); }
        }

        /// <summary>
        /// Number of open files that can be used by the DB.  You may need to
        /// increase this if your database has a large working set (budget
        /// one open file per 2MB of working set).
        ///
        /// Default: 1000
        /// </summary>
        public int MaxOpenFiles
        {
            set { Native.leveldb_options_set_max_open_files(Handle, value); }
        }

        /// <summary>
        /// Approximate size of user data packed per block.  Note that the
        /// block size specified here corresponds to uncompressed data.  The
        /// actual size of the unit read from disk may be smaller if
        /// compression is enabled.  This parameter can be changed dynamically.
        ///
        /// Default: 4K
        /// </summary>
        public int BlockSize
        {
            set { Native.leveldb_options_set_block_size(Handle, (UIntPtr)value); }
        }

        /// <summary>
        /// Number of keys between restart points for delta encoding of keys.
        /// This parameter can be changed dynamically.
        /// Most clients should leave this parameter alone.
        ///
        /// Default: 16
        /// </summary>
        public int BlockRestartInterval
        {
            set { Native.leveldb_options_set_block_restart_interval(Handle, value); }
        }

        /// <summary>
        /// Compress blocks using the specified compression algorithm.
        /// This parameter can be changed dynamically.
        ///
        /// Default: kSnappyCompression, which gives lightweight but fast compression.
        ///
        /// Typical speeds of kSnappyCompression on an Intel(R) Core(TM)2 2.4GHz:
        ///    ~200-500MB/s compression
        ///    ~400-800MB/s decompression
        /// Note that these speeds are significantly faster than most
        /// persistent storage speeds, and therefore it is typically never
        /// worth switching to kNoCompression.  Even if the input data is
        /// incompressible, the kSnappyCompression implementation will
        /// efficiently detect that and will switch to uncompressed mode.
        /// </summary>
        public CompressionType Compression
        {
            set { Native.leveldb_options_set_compression(Handle, value); }
        }

        public IntPtr FilterPolicy
        {
            set { Native.leveldb_options_set_filter_policy(Handle, value); }
        }

        protected override void FreeUnManagedObjects()
        {
            Native.leveldb_options_destroy(Handle);
        }
    }
}
