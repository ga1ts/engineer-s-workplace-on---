namespace RemontKt.Data
{
    /// <summary>
    /// Клас для представления сгруппированых по типу неисправности заявок
    /// </summary>
   public class RequestByFaultType
    {
        public FaultType FaultType { get; set; }
        public int Count { get; set; }
    }
}
