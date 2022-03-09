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
    * Shared content between NamespaceRegistrationTransaction and EmbeddedNamespaceRegistrationTransaction.
    */
    [Serializable]
    public class NamespaceRegistrationTransactionBodyBuilder: ISerializer {

        /* Number of confirmed blocks you would like to rent the namespace for. Required for root namespaces.. */
        public BlockDurationDto? duration;
        /* Parent namespace identifier. Required for sub-namespaces.. */
        public NamespaceIdDto? parentId;
        /* Namespace identifier.. */
        public NamespaceIdDto id;
        /* Namespace registration type.. */
        public NamespaceRegistrationTypeDto registrationType;
        /* Namespace name.. */
        public byte[] name;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespaceRegistrationTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                long registrationTypeCondition = (long)stream.ReadInt64();
                id = NamespaceIdDto.LoadFromBinary(stream);
                registrationType = (NamespaceRegistrationTypeDto)Enum.ToObject(typeof(NamespaceRegistrationTypeDto), (byte)stream.ReadByte());
                var nameSize = stream.ReadByte();
                name = GeneratorUtils.ReadBytes(stream, nameSize);
                if (this.registrationType == NamespaceRegistrationTypeDto.ROOT) {
                    duration = new BlockDurationDto(registrationTypeCondition);
                }
                if (this.registrationType == NamespaceRegistrationTypeDto.CHILD) {
                    parentId = new NamespaceIdDto(registrationTypeCondition);
                }
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespaceRegistrationTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespaceRegistrationTransactionBodyBuilder.
        */
        public static NamespaceRegistrationTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespaceRegistrationTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param duration Number of confirmed blocks you would like to rent the namespace for. Required for root namespaces..
        * @param parentId Parent namespace identifier. Required for sub-namespaces..
        * @param id Namespace identifier..
        * @param registrationType Namespace registration type..
        * @param name Namespace name..
        */
        internal NamespaceRegistrationTransactionBodyBuilder(BlockDurationDto? duration, NamespaceIdDto? parentId, NamespaceIdDto id, NamespaceRegistrationTypeDto registrationType, byte[] name)
        {
            if (registrationType == NamespaceRegistrationTypeDto.ROOT) {
                GeneratorUtils.NotNull(duration, "duration is null");
            }
            if (registrationType == NamespaceRegistrationTypeDto.CHILD) {
                GeneratorUtils.NotNull(parentId, "parentId is null");
            }
            GeneratorUtils.NotNull(id, "id is null");
            GeneratorUtils.NotNull(registrationType, "registrationType is null");
            GeneratorUtils.NotNull(name, "name is null");
            this.duration = duration;
            this.parentId = parentId;
            this.id = id;
            this.registrationType = registrationType;
            this.name = name;
        }
        
        /*
        * Creates an instance of NamespaceRegistrationTransactionBodyBuilder.
        *
        * @param duration Number of confirmed blocks you would like to rent the namespace for. Required for root namespaces..
        * @param id Namespace identifier..
        * @param name Namespace name..
        * @return Instance of NamespaceRegistrationTransactionBodyBuilder.
        */
        public static  NamespaceRegistrationTransactionBodyBuilder CreateROOT(BlockDurationDto duration, NamespaceIdDto id, byte[] name) {
            NamespaceRegistrationTypeDto registrationType = NamespaceRegistrationTypeDto.ROOT;
            return new NamespaceRegistrationTransactionBodyBuilder(duration, null, id, registrationType, name);
        }
        
        /*
        * Creates an instance of NamespaceRegistrationTransactionBodyBuilder.
        *
        * @param parentId Parent namespace identifier. Required for sub-namespaces..
        * @param id Namespace identifier..
        * @param name Namespace name..
        * @return Instance of NamespaceRegistrationTransactionBodyBuilder.
        */
        public static  NamespaceRegistrationTransactionBodyBuilder CreateCHILD(NamespaceIdDto parentId, NamespaceIdDto id, byte[] name) {
            NamespaceRegistrationTypeDto registrationType = NamespaceRegistrationTypeDto.CHILD;
            return new NamespaceRegistrationTransactionBodyBuilder(null, parentId, id, registrationType, name);
        }

        /*
        * Gets Number of confirmed blocks you would like to rent the namespace for. Required for root namespaces..
        *
        * @return Number of confirmed blocks you would like to rent the namespace for. Required for root namespaces..
        */
        public BlockDurationDto? GetDuration() {
            if (!(registrationType == NamespaceRegistrationTypeDto.ROOT)) {
                throw new Exception("registrationType is not set to ROOT.");
            }
            return duration;
        }

        /*
        * Gets Parent namespace identifier. Required for sub-namespaces..
        *
        * @return Parent namespace identifier. Required for sub-namespaces..
        */
        public NamespaceIdDto? GetParentId() {
            if (!(registrationType == NamespaceRegistrationTypeDto.CHILD)) {
                throw new Exception("registrationType is not set to CHILD.");
            }
            return parentId;
        }

        /*
        * Gets Namespace identifier..
        *
        * @return Namespace identifier..
        */
        public NamespaceIdDto GetId() {
            return id;
        }

        /*
        * Gets Namespace registration type..
        *
        * @return Namespace registration type..
        */
        public NamespaceRegistrationTypeDto GetRegistrationType() {
            return registrationType;
        }

        /*
        * Gets Namespace name..
        *
        * @return Namespace name..
        */
        public byte[] GetName() {
            return name;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            if (registrationType == NamespaceRegistrationTypeDto.ROOT) {
                if (duration != null) {
                size += ((BlockDurationDto) duration).GetSize();
            }
            }
            if (registrationType == NamespaceRegistrationTypeDto.CHILD) {
                if (parentId != null) {
                size += ((NamespaceIdDto) parentId).GetSize();
            }
            }
            size += id.GetSize();
            size += registrationType.GetSize();
            size += 1; // nameSize
            size += name.Length;
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
            if (registrationType == NamespaceRegistrationTypeDto.ROOT) {
                var durationEntityBytes = ((BlockDurationDto)duration).Serialize();
            bw.Write(durationEntityBytes, 0, durationEntityBytes.Length);
            }
            if (registrationType == NamespaceRegistrationTypeDto.CHILD) {
                var parentIdEntityBytes = ((NamespaceIdDto)parentId).Serialize();
            bw.Write(parentIdEntityBytes, 0, parentIdEntityBytes.Length);
            }
            var idEntityBytes = (id).Serialize();
            bw.Write(idEntityBytes, 0, idEntityBytes.Length);
            var registrationTypeEntityBytes = (registrationType).Serialize();
            bw.Write(registrationTypeEntityBytes, 0, registrationTypeEntityBytes.Length);
            bw.Write((byte)GeneratorUtils.GetSize(GetName()));
            bw.Write(name, 0, name.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
