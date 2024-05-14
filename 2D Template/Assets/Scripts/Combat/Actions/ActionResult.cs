public class ActionResult {
    public enum ResultType
    {
        Damage,
        Buff,
        Debuff,
        Heal,
        Miss
    }

    public ResultType resultType;
    public int value = 0;
    public string message;
    public bool stun = false;
    public ElementEnum.ElementType element;
    public CharacterCombat user, target;

    public ActionResult(CharacterCombat user, ResultType resultType, int value)
    {
        this.user = user;
        this.resultType = resultType;
        this.value = value;
    }
}