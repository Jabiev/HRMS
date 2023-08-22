namespace CareerCompassAPI.Domain.Stripe
{
    public record AddStripeCustomer(string Email, string Name, AddStripeCard CreditCard);
}
