namespace GensouGL.Helpers
{
    public static class HashHelper
    {
        private static string RotateShift(string input)
        {
            return input.Length > 1
                ? input[1..] + input[0]
                : input;
        }

        public static string ComputeSha256Hash(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(byte[] input)
        {
            var hashBytes = System.Security.Cryptography.SHA256.HashData(input);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(int input)
        {
            var bytes = BitConverter.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(long input)
        {
            var bytes = BitConverter.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(float input)
        {
            var bytes = BitConverter.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(double input)
        {
            var bytes = BitConverter.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(byte value)
        {
            var bytes = new byte[] { value };
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }

        public static string ComputeSha256Hash(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return RotateShift(Convert.ToHexString(hashBytes).ToLowerInvariant());
        }
    }
}