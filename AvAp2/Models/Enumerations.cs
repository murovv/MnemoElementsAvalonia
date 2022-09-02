using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvAp2
{
    public enum TagValueQuality
    {
        [Description("Хорошее")]
        Good = 0,
        [Description("Блокировка")]
        Blocking = 1,
        [Description("Замещение")]
        Replacement = 2,
        [Description("Ручной ввод")]
        Handled = 3,
        [Description("Неактуальное")]
        Irrelevant = 4,
        [Description("Недействительное")]
        Invalid = 8
    }


    public enum CommutationDeviceTypes
    {
        [Description("Автоматический выключатель")]
        AutomaticSwitch,

        [Description("Разъединитель")]
        IsolatingSwitch,
        [Description("Заземляющий нож")]
        PESwitch,
        [Description("Выкатной элемент 1")]
        CellCart1,
        [Description("Выкатной элемент 2")]
        CellCart2
    }

    public enum CommutationDeviceStates
    {
        #region Состояния КА
        [Description("Неопределенное")]
        UnDefined,
        [Description("Отключено")]
        Off,
        [Description("Включено")]
        On,
        [Description("Неисправность")]
        Broken,
        [Description("Выведен из работы")]
        OutOfWork,
        [Description("Включается")]
        TurningOn,
        [Description("Отключается")]
        TurningOff,
        [Description("Ручное заземление")]
        ManualPE
        #endregion Состояния КА
    }
}
