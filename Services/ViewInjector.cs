using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Reflection;
using Android.Support.Design.Widget;

namespace CarSto.Services
{
    public class ViewInjectorHelper
    {
        /// <summary>
        /// Основная ебала, включающая resourceId,
        /// ссылющийся на View.
        /// </summary>
        public class BaseInjectionAttribute : Attribute
        {
            public int ResourceId { get; private set; }

            public BaseInjectionAttribute(int resourceId)
            {
                ResourceId = resourceId;
            }
        }

        /// <summary>
        /// Инъекция view аттрибут. Виджеты, помеченный InjectView флагом
        /// ресолвятся по resourceId. Применим только к виджетам как полям класса.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class InjectView : BaseInjectionAttribute
        {
            public InjectView(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика click для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, EventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnClick : BaseInjectionAttribute
        {
            public InjectOnClick(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика checked для CompoundButton View.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, CompoundButton.CheckedChangeEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnCheckedChange : BaseInjectionAttribute
        {
            public InjectOnCheckedChange(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция action обработчика для TextView.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, TextView.EditorActionEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnEditorAction : BaseInjectionAttribute
        {
            public InjectOnEditorAction(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика focus changed для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, View.FocusChangeEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnFocusChange : BaseInjectionAttribute
        {
            public InjectOnFocusChange(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика item click для AdapterView.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, AdapterView.ItemClickEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnItemClick : BaseInjectionAttribute
        {
            public InjectOnItemClick(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика long item click для AdapterView.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, AdapterView.ItemLongClickEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnItemLongClick : BaseInjectionAttribute
        {
            public InjectOnItemLongClick(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика long item click для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, View.LongClickEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnLongClick : BaseInjectionAttribute
        {
            public InjectOnLongClick(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика touch для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, View.TouchEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnTouch : BaseInjectionAttribute
        {
            public InjectOnTouch(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обратчика text changed для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, Android.Text.TextChangedEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnTextChanged : BaseInjectionAttribute
        {
            public InjectOnTextChanged(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обратчика after text changed для view.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, Android.Text.AfterTextChangedEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnAfterTextChanged : BaseInjectionAttribute
        {
            public InjectOnAfterTextChanged(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика item selected для Spinner View.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, Spinner.ItemSelectedEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnItemSelected : BaseInjectionAttribute
        {
            public InjectOnItemSelected(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика Date Set для DatePicker.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, DatePickerDialog.DateSetEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnDateSet : BaseInjectionAttribute
        {
            public InjectOnDateSet(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика NavigationItemSelected для BottomNavigationView.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnNavigationItemSelected : BaseInjectionAttribute
        {
            public InjectOnNavigationItemSelected(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Инъекция обработчика CheckedChange для RadioGroup.
        /// Шаблон метода:
        /// <para></para><para></para>
        /// void SomeMethodName(object sender, RadioGroup.CheckedChangeEventArgs e) { ... }
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class InjectOnRadioGroupCheckedChange : BaseInjectionAttribute
        {
            public InjectOnRadioGroupCheckedChange(int resourceId) : base(resourceId) { }
        }

        /// <summary>
        /// Исключение при неправильном маппинге инжекшенов
        /// </summary>
        public class ViewInjectorException : Exception
        {
            const string PREFIX = "ViewInjector Exception: ";

            /// <summary>
            /// Вызов конструктора класса View и имени UI события, которое несовместимо с данным.
            /// </summary>
            /// <param name="viewType">Тип View.</param>
            /// <param name="eventName">Имя события.</param>
            public ViewInjectorException(Type viewType, string eventName) : base(GetViewTypeExceptionMessage(viewType, eventName)) { }

            /// <summary>
            /// Вызывает конструктор со списком требуемых параметров событий
            /// для индентефикации, что параметр не обнаружен или не совпадает
            /// с шаблоном вызова событий.
            /// </summary>
            /// <param name="requiredEventParameters">Требуемый тип события.</param>
            public ViewInjectorException(Type[] requiredEventParameters) : base(GetArgumentTypeExceptionMessage(requiredEventParameters)) { }

            /// <summary>
            /// Получение сообщения исключения о
            /// совместимоси с нативным типом события.
            /// </summary>
            /// <returns>Сообщения исключения.</returns>
            /// <param name="viewType">Тип View.</param>
            /// <param name="eventName">Имя события.</param>
            static string GetViewTypeExceptionMessage(Type viewType, string eventName)
            {
                var sb = new StringBuilder();
                sb.Append(PREFIX);
                sb.Append(" Данный тип View несовместим с '");
                sb.Append(eventName);
                sb.Append("', объект View '");
                sb.Append(viewType.ToString());
                sb.Append("' не поддерживает данное событие.");
                return sb.ToString();
            }

            static string GetArgumentTypeExceptionMessage(Type[] requiredEventParameters)
            {
                var sb = new StringBuilder();
                sb.Append(PREFIX);
                sb.Append(" Некорретный аргумент метода, укажите => (");
                for (var i = 0; i < requiredEventParameters.Length; i++)
                {
                    sb.Append(requiredEventParameters[i].ToString());
                    if (i < requiredEventParameters.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(")");
                return sb.ToString();
            }
        }

        public static class ViewInjector
        {
            #region EVENT / METHOD CONSTANTS
            const string METHOD_NAME_INVOKE = "Invoke";
            const string METHOD_NAME_RESOLVE_ANDROID_VIEW = "ResolveAndroidView";
            #endregion

            #region PRIVATE CONSTANTS
            const BindingFlags INJECTION_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            #endregion

            #region PUBLIC API
            public static void Inject(Activity parent)
            {
                InjectView(parent, parent.Window.DecorView.RootView);
            }

            public static void Inject(object parent, View view)
            {
                InjectView(parent, view);
            }

            public static void Reset(object parent)
            {
                foreach (var field in GetAttributedFields(typeof(InjectView), parent))
                {
                    field.SetValue(parent, null);
                }

                foreach (var property in GetAttributedProperties(typeof(InjectView), parent))
                {
                    property.SetValue(parent, null);
                }
            }
            #endregion

            #region PRIVATE API
            [Preserve]
            static void InjectionEventPreserver()
            {
                new View(null).Click += (s, e) => { };
                new View(null).LongClick += (s, e) => { };
                new View(null).FocusChange += (s, e) => { };
                new View(null).Touch += (s, e) => { };
                new TextView(null).EditorAction += (s, e) => { };
                new TextView(null).TextChanged += (s, e) => { };
                new ListView(null).ItemClick += (s, e) => { };
                new ListView(null).ItemLongClick += (s, e) => { };
                new CheckBox(null).CheckedChange += (s, e) => { };
                new EditText(null).AfterTextChanged += (s, e) => { };
                new Spinner(null).ItemSelected += (s, e) => { };
                new DatePickerDialog(null).DateSet += (s, e) => { };
                new RadioGroup(null).CheckedChange += (s, e) => { };
                new BottomNavigationView(null).NavigationItemSelected += (s, e) => { };
            }

            static Dictionary<Type, string> GetInjectableEvents()
            {
                var types = new Dictionary<Type, string>
                {
                    { typeof(InjectOnClick), "Click" },
                    { typeof(InjectOnItemClick), "ItemClick" },
                    { typeof(InjectOnLongClick), "LongClick" },
                    { typeof(InjectOnItemLongClick), "ItemLongClick" },
                    { typeof(InjectOnFocusChange), "FocusChange" },
                    { typeof(InjectOnCheckedChange), "CheckedChange" },
                    { typeof(InjectOnEditorAction), "EditorAction" },
                    { typeof(InjectOnTouch), "Touch" },
                    { typeof(InjectOnTextChanged), "TextChanged" },
                    { typeof(InjectOnAfterTextChanged), "AfterTextChanged" },
                    { typeof(InjectOnItemSelected), "ItemSelected" },
                    { typeof(InjectOnDateSet), "DateSet" },
                    { typeof(InjectOnNavigationItemSelected), "NavigationItemSelected" },
                    { typeof(InjectOnRadioGroupCheckedChange), "CheckedChange" }
                };
                return types;
            }

            static IEnumerable<FieldInfo> GetAttributedFields(Type attributeType, object parent)
            {
                return parent.GetType().GetFields(INJECTION_BINDING_FLAGS).Where(x => x.IsDefined(attributeType));
            }

            static IEnumerable<PropertyInfo> GetAttributedProperties(Type attributeType, object parent)
            {
                return parent.GetType().GetProperties(INJECTION_BINDING_FLAGS).Where(x => x.IsDefined(attributeType));
            }

            static IEnumerable<MethodInfo> GetAttributedMethods(Type attributeType, object parent)
            {
                return parent.GetType().GetMethods(INJECTION_BINDING_FLAGS).Where(x => x.IsDefined(attributeType));
            }

            static T ResolveAndroidView<T>(View view, int resourceId) where T : View
            {
                return view.FindViewById<T>(resourceId);
            }

            static void InjectView(object parent, View view)
            {
                var resolveMethod = typeof(ViewInjector).GetMethod(METHOD_NAME_RESOLVE_ANDROID_VIEW, BindingFlags.Static | BindingFlags.NonPublic);

                foreach (var field in GetAttributedFields(typeof(InjectView), parent))
                {
                    var attribute = field.GetCustomAttribute<InjectView>();
                    var genericMethod = resolveMethod.MakeGenericMethod(field.FieldType);
                    var widget = genericMethod.Invoke(parent, new object[] { view, attribute.ResourceId });
                    field.SetValue(parent, widget);
                }

                foreach (var property in GetAttributedProperties(typeof(InjectView), parent))
                {
                    var attribute = property.GetCustomAttribute<InjectView>();
                    var genericMethod = resolveMethod.MakeGenericMethod(property.PropertyType);
                    var widget = genericMethod.Invoke(parent, new object[] { view, attribute.ResourceId });
                    property.SetValue(parent, widget);
                }

                foreach (var injectableEvent in GetInjectableEvents())
                {
                    var attributeType = injectableEvent.Key;
                    var eventName = injectableEvent.Value;
                    var methods = GetAttributedMethods(attributeType, parent);
                    foreach (var method in methods)
                    {
                        InjectMethod(attributeType, method, parent, view, eventName);
                    }
                }
            }

            static void InjectMethod(Type attributeType, MethodInfo method, object parent, View view, string eventName)
            {                
                var attribute = method.GetCustomAttribute(attributeType, false) as BaseInjectionAttribute;
                if (attribute == null)
                {
                    return;
                }
                
                var widget = view.FindViewById<View>(attribute.ResourceId);
               
                var eventInfo = widget.GetType().GetEvent(eventName);
                if (eventInfo == null)
                {
                    throw new ViewInjectorException(widget.GetType(), eventName);
                }
                
                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var eventParameterTypes = eventInfo.EventHandlerType.GetMethod(METHOD_NAME_INVOKE).GetParameters().Select(p => p.ParameterType).ToArray();
              
                if (methodParameterTypes.Length != eventParameterTypes.Length)
                {
                    throw new ViewInjectorException(eventParameterTypes);
                }

                for (var i = 0; i < methodParameterTypes.Length; i++)
                {
                    if (methodParameterTypes[i] != eventParameterTypes[i])
                    {
                        throw new ViewInjectorException(eventParameterTypes);
                    }
                }

                var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, parent, method);
                eventInfo.AddEventHandler(widget, handler);
            }
            #endregion
        }
    }
}