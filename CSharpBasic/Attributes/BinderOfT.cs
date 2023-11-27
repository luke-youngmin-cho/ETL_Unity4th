using System.ComponentModel;
using System.Reflection;

namespace Attributes
{
    internal class Binder<T>
    {
        private readonly T _receiver;
        private readonly PropertyDescriptorCollection _sourceProperties;
        private readonly PropertyDescriptorCollection _receiverMappedProperties;
        /// <summary>
        /// </summary>
        /// <param name="receiver"> 알림을 받을 객체 (View) </param>
        /// <param name="source"> 알림을 고지하는 데이터 (Model) </param>
        /// <param name="tag"> 소스를 이름외에도 구분하기위한 용도 </param>
        public Binder(T receiver, INotifyPropertyChanged source, SourceTag tag)
        {
            _receiver = receiver;

            PropertyDescriptorCollection receiverProperties = TypeDescriptor.GetProperties(_receiver); // GoldUI 의 프로퍼티 정보 다 읽음
            _sourceProperties = TypeDescriptor.GetProperties(source); // GoldViewModel 의 프로퍼티 정보 다 읽음

            source.PropertyChanged += OnSourcePropertyChanged; // GoldViewModel 의 데이터가 바뀌었을때, 실행할 내용 구독

            // Bind Attribute 가지고있으면서, 태그가 일치하는 Receiver 의 Property 만 저장
            _receiverMappedProperties = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor property in receiverProperties)
            {
                BindAttribute attribute = (BindAttribute)property.Attributes[typeof(BindAttribute)];
                if (attribute != null && attribute.Tag == tag) // Bind Attribute 가 붙어있으면서 Tag 가 동일한 프로퍼티 맵핑
                {
                    _receiverMappedProperties.Add(property);
                }
            }
        }

        /// <summary>
        /// 소스(Sender)의 값이 바뀌었을때, 매핑된 Receiver 가 있다면, Sender 의 Property 의 값으로 Recevier 의 Property 값을 갱신.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            PropertyDescriptor? property = _receiverMappedProperties[args.PropertyName];
            if (property != null)
            {
                property.SetValue(_receiver, _sourceProperties[args.PropertyName].GetValue(sender));
            }
        }

    }
}
