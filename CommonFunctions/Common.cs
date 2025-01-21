namespace KoperasiTenteraApi.CommonFunctions
{
    public static class Common
    {

        public static string GenerateOtp(int length)
        {
            var random = new Random();
            string otp = random.Next(0, (int)Math.Pow(10, length)).ToString($"D{length}");
            return otp;
        }
    }
}
