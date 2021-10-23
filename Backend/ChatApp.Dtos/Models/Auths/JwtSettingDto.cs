namespace ChatApp.Dtos.Models.Auths
{
    public class JwtSettingDto
    {
        public string Secret { get; set; }

        public int ExpiredInDays { get; set; }
    }
}
