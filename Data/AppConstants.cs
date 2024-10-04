namespace stocks.Data;

public static class AppConstants
{
    public static string UserSessionData = "UserId";

    public static class OpenAi
    {
        public static string RequestSocialRatingText = $"Give me the social reputation of the company on stock market represented by a symbol, as json object, one property would be the symbol, the other property the reputation out of 10 and the last property would be a list of facts that explains the rating, including real behaviours to prove some points, these real behaviours needs to be concrete actions taken by the company. Also the response needs to be a json only, no text and the whole response provided must not contain any special characters since it will be deserialized in a program, even the facts. The symbol is : ";
        public static string GptModel = "gpt-4o-mini";
        public static decimal GptQuestionTemperature = 0.99m;
        public static string UserRole = "user";
    }
}