using SonarNet;

namespace SonarNet.Tests
{
    public class UserAuthTests
    {
        [Fact]
        public void Login_SucceedsWithCorrectPassword()
        {
            var auth = new UserAuth();
            Assert.True(auth.Register("oleg", "Secret123"));
            Assert.True(auth.Login("oleg", "Secret123"));
        }

        [Fact]
        public void Login_IsCaseSensitive()
        {
            var auth = new UserAuth();
            auth.Register("oleg", "Secret123");
            Assert.False(auth.Login("oleg", "secret123"));
        }

        [Fact]
        public void Register_RejectsShortPassword()
        {
            Assert.False(new UserAuth().Register("oleg", "short"));
        }

        [Fact]
        public void Account_IsLockedAfterThreeFailedAttempts()
        {
            var auth = new UserAuth();
            auth.Register("oleg", "Secret123");
            for (int i = 0; i < 3; i++)
            {
                auth.Login("oleg", "wrong-password");
            }
            Assert.True(auth.IsLocked("oleg"));
            Assert.False(auth.Login("oleg", "Secret123"));
        }
    }

    public class NewFeatureTests
    {
        [Theory]
        [InlineData(440, false, 44)]
        [InlineData(440, true, 88)]
        [InlineData(9, false, 0)]
        [InlineData(-5, true, 0)]
        public void LoyaltyPoints_AreCalculatedFromTotal(decimal total, bool vip, int expected)
        {
            Assert.Equal(expected, NewFeature.LoyaltyPoints(total, new Customer { IsVip = vip }));
        }
    }
}
