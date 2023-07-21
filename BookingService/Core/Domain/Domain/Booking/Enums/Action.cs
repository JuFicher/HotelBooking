namespace Domain.Enums
{
    public enum Action
    {
        Pay = 0,
        Finish = 1, //após ter pago e utilizado o seviço
        Cancel = 2, // reserva não chegou a ser paga
        Refound = 3, // reserva foi paga e será reembolsada
        Reopen = 4 // reserva status Cancel que será refeita
    }
}