using AvAp2.Models;

namespace AvAp2.Interfaces
{
    /// <summary>
    /// Подразумевает наличие у элемента общего изменяемого состояния, например, значение тега у текущих данных, наличие связи у порта, наличие напряжения у оборудования
    /// </summary>
    public interface IBasicWithState
    {
        /// <summary>
        /// ID тега для привязки состояния
        /// </summary>
        string TagIDMainState { get; set; }

        /// <summary>
        /// В рантайме ссылка на привязанный объект, поставляющий данные состояния
        /// </summary>
        TagDataItem TagDataMainState { get; }
    }
}