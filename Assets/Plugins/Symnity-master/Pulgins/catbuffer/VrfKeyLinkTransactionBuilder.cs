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
    * Link an account with a VRF public key required for harvesting.
Announce a VrfKeyLinkTransaction to link an account with a VRF public key. The linked key is used to randomize block production and leader/participant selection.
This transaction is required for all accounts wishing to [harvest](/concepts/harvesting.html).
    */
    [Serializable]
    public class VrfKeyLinkTransactionBuilder: TransactionBuilder {

        /* Vrf key link transaction body. */
        public VrfKeyLinkTransactionBodyBuilder vrfKeyLinkTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal VrfKeyLinkTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                vrfKeyLinkTransactionBody = VrfKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of VrfKeyLinkTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of VrfKeyLinkTransactionBuilder.
        */
        public new static VrfKeyLinkTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new VrfKeyLinkTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param linkedPublicKey Linked VRF public key..
        * @param linkAction Account link action..
        */
        internal VrfKeyLinkTransactionBuilder(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, PublicKeyDto linkedPublicKey, LinkActionDto linkAction)
            : base(signature, signerPublicKey, version, network, type, fee, deadline)
        {
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(fee, "fee is null");
            GeneratorUtils.NotNull(deadline, "deadline is null");
            GeneratorUtils.NotNull(linkedPublicKey, "linkedPublicKey is null");
            GeneratorUtils.NotNull(linkAction, "linkAction is null");
            this.vrfKeyLinkTransactionBody = new VrfKeyLinkTransactionBodyBuilder(linkedPublicKey, linkAction);
        }
        
        /*
        * Creates an instance of VrfKeyLinkTransactionBuilder.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param linkedPublicKey Linked VRF public key..
        * @param linkAction Account link action..
        * @return Instance of VrfKeyLinkTransactionBuilder.
        */
        public static  VrfKeyLinkTransactionBuilder Create(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, PublicKeyDto linkedPublicKey, LinkActionDto linkAction) {
            return new VrfKeyLinkTransactionBuilder(signature, signerPublicKey, version, network, type, fee, deadline, linkedPublicKey, linkAction);
        }

        /*
        * Gets Linked VRF public key..
        *
        * @return Linked VRF public key..
        */
        public PublicKeyDto GetLinkedPublicKey() {
            return vrfKeyLinkTransactionBody.GetLinkedPublicKey();
        }

        /*
        * Gets Account link action..
        *
        * @return Account link action..
        */
        public LinkActionDto GetLinkAction() {
            return vrfKeyLinkTransactionBody.GetLinkAction();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += vrfKeyLinkTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new VrfKeyLinkTransactionBodyBuilder GetBody() {
            return vrfKeyLinkTransactionBody;
        }


    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public new byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var vrfKeyLinkTransactionBodyEntityBytes = (vrfKeyLinkTransactionBody).Serialize();
            bw.Write(vrfKeyLinkTransactionBodyEntityBytes, 0, vrfKeyLinkTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
