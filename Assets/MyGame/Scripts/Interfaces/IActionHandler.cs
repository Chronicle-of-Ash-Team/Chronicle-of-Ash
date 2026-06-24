public interface IActionHandler
{
    public void TryAction(BaseCombatAction action);
    public void OnActionFinished(BaseCombatAction action);
}
