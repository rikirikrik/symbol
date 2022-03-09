/**
*** Copyright (c) 2016-2019, Jaguar0625, gimre, BloodyRookie, Tech Bureau, Corp.
*** Copyright (c) 2020-present, Jaguar0625, gimre, BloodyRookie.
*** All rights reserved.
***
*** This file is part of Catapult.
***
*** Catapult is free software: you can redistribute it and/or modify
*** it under the terms of the GNU Lesser General Public License as published by
*** the Free Software Foundation, either version 3 of the License, or
*** (at your option) any later version.
***
*** Catapult is distributed in the hope that it will be useful,
*** but WITHOUT ANY WARRANTY; without even the implied warranty of
*** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*** GNU Lesser General Public License for more details.
***
*** You should have received a copy of the GNU Lesser General Public License
*** along with Catapult. If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Symbol.Builders {
    /*
    * Shared content between MosaicMetadataTransaction and EmbeddedMosaicMetadataTransaction.
    */
    [Serializable]
    public class MosaicMetadataTransactionBodyBuilder: ISerializer {

        /* Account owning the mosaic whose metadata should be modified.. */
        public UnresolvedAddressDto targetAddress;
        /* Metadata key scoped to source, target and type.. */
        public long scopedMetadataKey;
        /* Mosaic whose metadata should be modified.. */
        public UnresolvedMosaicIdDto targetMosaicId;
        /* Change in value size in bytes, compared to previous size.. */
        public short valueSizeDelta;
        /* Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array.. */
        public byte[] value;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicMetadataTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                targetAddress = UnresolvedAddressDto.LoadFromBinary(stream);
                scopedMetadataKey = stream.ReadInt64();
                targetMosaicId = UnresolvedMosaicIdDto.LoadFromBinary(stream);
                valueSizeDelta = stream.ReadInt16();
                var valueSize = stream.ReadInt16();
                value = GeneratorUtils.ReadBytes(stream, valueSize);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicMetadataTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicMetadataTransactionBodyBuilder.
        */
        public static MosaicMetadataTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicMetadataTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param targetAddress Account owning the mosaic whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetMosaicId Mosaic whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        internal MosaicMetadataTransactionBodyBuilder(UnresolvedAddressDto targetAddress, long scopedMetadataKey, UnresolvedMosaicIdDto targetMosaicId, short valueSizeDelta, byte[] value)
        {
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            GeneratorUtils.NotNull(scopedMetadataKey, "scopedMetadataKey is null");
            GeneratorUtils.NotNull(targetMosaicId, "targetMosaicId is null");
            GeneratorUtils.NotNull(valueSizeDelta, "valueSizeDelta is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.targetAddress = targetAddress;
            this.scopedMetadataKey = scopedMetadataKey;
            this.targetMosaicId = targetMosaicId;
            this.valueSizeDelta = valueSizeDelta;
            this.value = value;
        }
        
        /*
        * Creates an instance of MosaicMetadataTransactionBodyBuilder.
        *
        * @param targetAddress Account owning the mosaic whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetMosaicId Mosaic whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        * @return Instance of MosaicMetadataTransactionBodyBuilder.
        */
        public static  MosaicMetadataTransactionBodyBuilder Create(UnresolvedAddressDto targetAddress, long scopedMetadataKey, UnresolvedMosaicIdDto targetMosaicId, short valueSizeDelta, byte[] value) {
            return new MosaicMetadataTransactionBodyBuilder(targetAddress, scopedMetadataKey, targetMosaicId, valueSizeDelta, value);
        }

        /*
        * Gets Account owning the mosaic whose metadata should be modified..
        *
        * @return Account owning the mosaic whose metadata should be modified..
        */
        public UnresolvedAddressDto GetTargetAddress() {
            return targetAddress;
        }

        /*
        * Gets Metadata key scoped to source, target and type..
        *
        * @return Metadata key scoped to source, target and type..
        */
        public long GetScopedMetadataKey() {
            return scopedMetadataKey;
        }

        /*
        * Gets Mosaic whose metadata should be modified..
        *
        * @return Mosaic whose metadata should be modified..
        */
        public UnresolvedMosaicIdDto GetTargetMosaicId() {
            return targetMosaicId;
        }

        /*
        * Gets Change in value size in bytes, compared to previous size..
        *
        * @return Change in value size in bytes, compared to previous size..
        */
        public short GetValueSizeDelta() {
            return valueSizeDelta;
        }

        /*
        * Gets Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        *
        * @return Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        public byte[] GetValue() {
            return value;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += targetAddress.GetSize();
            size += 8; // scopedMetadataKey
            size += targetMosaicId.GetSize();
            size += 2; // valueSizeDelta
            size += 2; // valueSize
            size += value.Length;
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var targetAddressEntityBytes = (targetAddress).Serialize();
            bw.Write(targetAddressEntityBytes, 0, targetAddressEntityBytes.Length);
            bw.Write(GetScopedMetadataKey());
            var targetMosaicIdEntityBytes = (targetMosaicId).Serialize();
            bw.Write(targetMosaicIdEntityBytes, 0, targetMosaicIdEntityBytes.Length);
            bw.Write(GetValueSizeDelta());
            bw.Write((short)GeneratorUtils.GetSize(GetValue()));
            bw.Write(value, 0, value.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
