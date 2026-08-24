public static class DamageCalculator
{
    public static int CalculateDamage(int baseDamage)
    {
        if (baseDamage <= 0)
        {
            return 0;
        }

        return baseDamage;
    }
}
