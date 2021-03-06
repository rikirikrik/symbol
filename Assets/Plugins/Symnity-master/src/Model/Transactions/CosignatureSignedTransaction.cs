namespace Symnity.Model.Transactions
{
    public class CosignatureSignedTransaction
    {
        /**
         * The hash of parent aggregate transaction that has been signed by a cosignatory of the transaction
         */
        public readonly string ParentHash;

        /**
         * The signatures generated by signing the parent aggregate transaction hash.
         */
        public readonly string Signature;

        /**
         * The signer publicKey of the transaction.
         */
        public readonly string SignerPublicKey;

        /**
         * Version
         */
        public readonly int Version;

        /**
         * @param parentHash
         * @param signature
         * @param signerPublicKey
         * @param version
         */
        public CosignatureSignedTransaction(
            string parentHash,
            string signature,
            string signerPublicKey,
            int version = 0
        )
        {
            ParentHash = parentHash;
            Signature = signature;
            SignerPublicKey = signerPublicKey;
            Version = version;
        }
    }
}