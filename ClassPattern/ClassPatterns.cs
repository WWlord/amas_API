using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassPattern
{
    class CommonPatterns
    {
        /// <summary>

        /// Реализация шаблона Singleton
        /// Шаблон Singleton создает класс, для которого на протяжении всего выполнения приложения создается единственный экземпляр объекта, 
        /// поэтому всякий раз, когда любой другой объект пытается обратиться к объекту этого класса, он всегда обращается именно к этому единственному объекту.
        /// </summary>

        public sealed class SingleTon
        {

            private static SingleTon _instance = null;

            private SingleTon() // конструктор по умолчанию определяется как private
            {

            }

            /// <summary>

            /// Единственный экземпляр

            /// </summary>

            public static SingleTon Instance
            {

                get
                {

                    lock (_instance)
                    {

                        _instance = _instance ?? new SingleTon();

                        return _instance;

                    }

                }

            }

            # region Rest of Implementation Logic

            // Добавьте здесь столько методов, сколько нужно, в качестве члена экземпляра. Не нужно делать их статическими (static)

            # endregion

        }


        /// <summary>
        /// Шаблон фабрики (Factory)
        /// Шаблон фабрики связан с созданием экземпляра объекта без предоставления логики создания экземпляра. 
        /// Другими словами, фабрика фактически является создателем объекта с общим интерфейсом. 
        /// В приведенном ниже коде можно видеть, что я создал один интерфейс, названный IPeople, и реализовал его в двух классах, Villagers и CityPeople. 
        /// В зависимости от типа, переданного в объект фабрики, я возвращаю исходный конкретный объект как интерфейс IPeople.
        /// </summary>
        
        //Пустой словарь фактического объекта

        public interface IPeople
        {

            string GetName();

        }

        public class Villagers : IPeople
        {

            #region IPeople Members

            public string GetName()
            {

                return "Village Guy";

            }

            #endregion

        }

        public class CityPeople : IPeople
        {

            #region IPeople Members

            public string GetName()
            {

                return "City Guy";

            }

            #endregion

        }

        public enum PeopleType
        {

            RURAL,

            URBAN

        }

        /// <summary>

        /// Реализация фабрики — используется для создания объектов

        /// </summary>

        public class Factory
        {

            public IPeople GetPeople(PeopleType type)
            {

                IPeople people = null;

                switch (type)
                {

                    case PeopleType.RURAL:

                        people = new Villagers();

                        break;

                    case PeopleType.URBAN:

                        people = new CityPeople();

                        break;

                    default:

                        break;

                }

                return people;

            }

        }


        /// <summary>
        /// Метод фабрики (Factory Method)
        /// Метод фабрики — это просто расширение класса фабрики (Factory). Он создает объект класса с помощью интерфейсов, но, с другой стороны, 
        /// он также позволяет подклассу определить, экземпляр какого класса будет создаваться.
        /// Можно видеть, что использован GetObject в concreteFactory. Это позволяет легко вызвать в данном классе DoSomething(), чтобы получить IProduct.
        /// </summary>

        public interface IProduct
        {

            string GetName();

            string SetPrice(double price);

        }



        public class IPhone : IProduct
        {

            private double _price;

            #region IProduct Members

            public string GetName()
            {

                return "Apple TouchPad";

            }

            public string SetPrice(double price)
            {

                this._price = price;

                return "success";

            }

            #endregion

        }



        /* Почти то же самое, что и Factory, просто предоставляет дополнительную возможность для выполнения каких-либо действий, используя созданный метод*/

        public abstract class ProductAbstractFactory
        {

            public IProduct DoSomething()
            {

                IProduct product = this.GetObject();

                // Выполните что-нибудь с объектом, получив объект.

                product.SetPrice(20.30);

                return product;

            }

            public abstract IProduct GetObject();

        }



        public class ProductConcreteFactory : ProductAbstractFactory
        {

            public override IProduct GetObject() // реализация метода фабрики.
            {

                return this.DoSomething();

            }

        }


        /// <summary>
        /// Абстрактная фабрика (Abstract Factory)
        /// Абстрактная фабрика — это расширение базового шаблона фабрики (Factory). 
        /// Он предоставляет интерфейсы Factory для создания семейства связанных классов. Другими словами, здесь я объявляю интерфейсы для фабрик, 
        /// которые, в свою очередь, будут работать так же, как с фабриками.
        /// </summary>

        public interface IFactory1
        {

            IPeople GetPeople();

        }



        public class Factory1 : IFactory1
        {

            public IPeople GetPeople()
            {

                return new Villagers();

            }

        }



        public interface IFactory2
        {

            IProduct GetProduct();

        }



        public class Factory2 : IFactory2
        {

            public IProduct GetProduct()
            {

                return new IPhone();

            }

        }



        public abstract class AbstractFactory12
        {

            public abstract IFactory1 GetFactory1();

            public abstract IFactory2 GetFactory2();

        }



        public class ConcreteFactory : AbstractFactory12
        {

            public override IFactory1 GetFactory1()
            {

                return new Factory1();

            }

            public override IFactory2 GetFactory2()
            {

                return new Factory2();

            }

        }

        /// <summary>
        /// Шаблон построителя (Builder)
        /// Этот шаблон создает объект на основе интерфейса, но также позволяет подклассу определить, экземпляр какого класса будет реализован. 
        /// Он также обеспечивает более тонкое управление процессом создания. В реализации шаблона построителя используется концепция директора (Director). 
        /// Директор фактически создает объект, а после этого также выполняет несколько задач. 
        /// В случае шаблона построителя можно видеть, что директор фактически использует CreateBuilder для создания экземпляра построителя. 
        /// Поэтому после фактического создания построителя мы также можем вызвать в нем несколько типовых задач.
        /// </summary>

        public interface IBuilder
        {

            string RunBulderTask1();

            string RunBuilderTask2();

        }



        public class Builder1 : IBuilder
        {

            #region IBuilder Members

            public string RunBulderTask1()
            {

                throw new ApplicationException("Task1");

            }

            public string RunBuilderTask2()
            {

                throw new ApplicationException("Task2");

            }

            #endregion

        }



        public class Builder2 : IBuilder
        {

            #region IBuilder Members

            public string RunBulderTask1()
            {

                return "Task3";

            }

            public string RunBuilderTask2()
            {

                return "Task4";

            }

            #endregion

        }



        public class Director
        {

            public IBuilder CreateBuilder(int type)
            {

                IBuilder builder = null;

                if (type == 1)

                    builder = new Builder1();

                else

                    builder = new Builder2();



                builder.RunBulderTask1();

                builder.RunBuilderTask2();

                return builder;

            }

        }


        /// <summary>
        /// Шаблон прототипа (Prototype)
        /// Этот шаблон создает разновидность объекта, используя его прототип. Другими словами, 
        /// при создании объекта Prototype класс фактически создает его клон и возвращает его в качестве прототипа.
        /// Как можно видеть, я, когда понадобилось, использовал метод MemberwiseClone для клонирования прототипа. 
        /// Необходимо помнить, что MemberwiseClone — это фактически только копия. Чтобы сделать копию более глубокой, 
        /// понадобится вызывать MemberwiseClone рекурсивно для каждого объекта, пока не будут реализованы его значения. 
        /// </summary>

        public abstract class Prototype
        {

            // обычная реализация

            public abstract Prototype Clone();

        }



        public class ConcretePrototype1 : Prototype
        {

            public override Prototype Clone()
            {

                return (Prototype)this.MemberwiseClone();

            }

        }



        class ConcretePrototype2 : Prototype
        {

            public override Prototype Clone()
            {

                return (Prototype)this.MemberwiseClone(); // клонирует конкретный класс.

            }

        }


        /// <summary>
        /// Шаблон моста (Bridge)
        /// Шаблон моста объединяет объекты в древовидную структуру. Он отделяет абстракцию от реализации. 
        /// Здесь абстракция — это клиент, из которого будут вызываться объекты.
        /// Таким образом, можно видеть, что классы Bridge являются реализацией, использующей ту же ориентированную на интерфейсы архитектуру 
        /// для создания объектов. С другой стороны, абстракция берет объект из фазы реализации и выполняет его метод. 
        /// Это позволяет полностью отделить одно от другого.
        /// </summary>


        # region The Implementation

        /// <summary>

        /// Помогает обеспечить действительно раздельную архитектуру

        /// </summary>

        public interface IBridge
        {

            void Function1();

            void Function2();

        }



        public class Bridge1 : IBridge
        {

            #region IBridge Members

            public void Function1()
            {

                throw new NotImplementedException();

            }

            public void Function2()
            {

                throw new NotImplementedException();

            }

            #endregion

        }



        public class Bridge2 : IBridge
        {

            #region IBridge Members

            public void Function1()
            {

                throw new NotImplementedException();

            }

            public void Function2()
            {

                throw new NotImplementedException();

            }

            #endregion

        }

        # endregion





        # region Abstraction

        public interface IAbstractBridge
        {

            void CallMethod1();

            void CallMethod2();

        }



        public class AbstractBridge : IAbstractBridge
        {

            public IBridge bridge;

            public AbstractBridge(IBridge bridge)
            {

                this.bridge = bridge;

            }

            #region IAbstractBridge Members

            public void CallMethod1()
            {

                this.bridge.Function1();

            }

            public void CallMethod2()
            {

                this.bridge.Function2();

            }

            #endregion

        }

        # endregion


        /// <summary>
        /// Шаблон декоратора (Decorator)
        /// Шаблон декоратора используется для динамического создания ответственностей. 
        /// Это означает, что в случае использования шаблона декоратора в каждый класс добавляются специальные характеристики. 
        /// Другими словами, шаблон декоратора — это то же самое, что и наследование.
        /// Это то же самое отношение родителя и потомка, в котором в класс-потомок добавляется новая функция, называемая Method2, 
        /// а другие характеристики наследуются от родителя.
        /// </summary>

        public class ParentClass
        {

            public void Method1()
            {

            }

        }

        public class DecoratorChild : ParentClass
        {

            public void Method2()
            {

            }

        }


        /// <summary>
        /// Шаблон составного компонента (Composite)
        /// Этот шаблон рассматривает компоненты как соединение одного или нескольких элементов, позволяя отделять компоненты друг от друга. 
        /// Другими словами, шаблоны составных компонентов — это шаблоны, из которых можно легко выделить отдельные элементы.
        /// В приведенном коде можно видеть, что в in NormalComposite элементы IComposite можно легко отделить.

        /// Рассматривает элементы как объединение одного или нескольких элементов, позволяя разделить компоненты
        /// один от другого

        /// </summary>

        public interface IComposite
        {

            void CompositeMethod();

        }



        public class LeafComposite : IComposite
        {

            #region IComposite Members

            public void CompositeMethod()
            {

                // Какие-нибудь действия

            }

            #endregion

        }



        /// <summary>

        /// Элементы интерфейса IComposite можно отделить от других элементов

        /// </summary>

        public class NormalComposite : IComposite
        {

            #region IComposite Members

            public void CompositeMethod()
            {

                // Какие-нибудь действия

            }

            #endregion

            public void DoSomethingMore()
            {

                // Еще какие-нибудь действия

            }

        }


        /// <summary>
        /// Шаблон маховика (Flyweight)
        /// Маховик позволяет совместно использовать объемные данные, общие для всех объектов. 
        /// Другими словами, если одни и те же данные повторяются для каждого из объектов, можно воспользоваться этим шаблоном, 
        /// чтобы указать на один объект и, таким образом, легко сэкономить место.
        /// Здесь FlyweightPointer создает статический член Company, используемый для каждого объекта класса MyObject.

        /// Определяет объект-маховик, повторяющий сам себя.

        /// </summary>

        public class FlyWeight
        {

            public string Company { get; set; }

            public string CompanyLocation { get; set; }

            public string CompanyWebSite { get; set; }

            // Объемные данные

            public byte[] CompanyLogo { get; set; }

        }



        public static class FlyWeightPointer
        {

            public static FlyWeight Company = new FlyWeight

            {

                Company = "Abc",

                CompanyLocation = "XYZ",

                CompanyWebSite = "www.abc.com"

            };

        }

        public class MyObject
        {

            public string Name { get; set; }

            public FlyWeight Company
            {

                get
                {

                    return FlyWeightPointer.Company;

                }

            }

        }


        /// <summary>
        /// Шаблон для запоминания состояния (Memento)
        /// Шаблон запоминания позволяет захватывать внутреннее состояние объекта, не нарушая инкапсуляцию, и позднее, в случае необходимости, 
        /// отменять/ откатывать внесенные изменения.
        /// Можно видеть, что объект Memento фактически используется, чтобы откатить (Revert) изменения, внесенные в объект.
        /// </summary>
        /// 
        public class OriginalObject
        {

            public string String1 { get; set; }

            public string String2 { get; set; }

            public Memento MyMemento { get; set; }



            public OriginalObject(string str1, string str2)
            {

                this.String1 = str1;

                this.String2 = str2;

                this.MyMemento = new Memento(str1, str2);

            }

            public void Revert()
            {

                this.String1 = this.MyMemento.String1;

                this.String2 = this.MyMemento.String2;

            }

        }



        public class Memento
        {

            public string String1 { get; set; }

            public string String2 { get; set; }

            public Memento(string str1, string str2)
            {

                this.String1 = str1;

                this.String2 = str2;

            }

        }


        /// <summary>
        /// Шаблон посредника (Mediator)
        /// Шаблон посредника гарантирует, что компоненты являются слабосвязанными, то есть они не вызывают друг друга напрямую, 
        /// всегда используя для решения такой задачи отдельно реализованный объект-посредник.
        /// Приведенный код показывает, как класс Mediator регистрирует все компоненты внутри себя, а затем вызывает свой метод, когда это требуется.
        /// </summary>

        public interface IComponent
        {

            void SetState(object state);

        }



        public class Component1 : IComponent
        {

            #region IComponent Members

            public void SetState(object state)
            {

                // Не выполяет никаких действий

                throw new NotImplementedException();

            }

            #endregion

        }



        public class Component2 : IComponent
        {

            #region IComponent Members

            public void SetState(object state)
            {

                // Не выполяет никаких действий

                throw new NotImplementedException();

            }

            #endregion

        }



        public class Mediator // Служит посредником для типовых задач
        {

            public IComponent Component1 { get; set; }

            public IComponent Component2 { get; set; }

            public void ChageState(object state)
            {

                this.Component1.SetState(state);

                this.Component2.SetState(state);

            }

            
        }


        /// <summary>
        /// Шаблон наблюдателя (Observer)
        /// Если несколько объектов связаны некоторыми отношениями, наблюдатель уведомит все зависимые элементы о каких-либо изменениях в родительском элементе. 
        /// Корпорация Майкрософт уже реализовала этот шаблон как коллекцию ObservableCollection. Я же здесь реализую самый базовый шаблон наблюдателя.
        /// Приведенный код позволяет получить представление о том, что после регистрации для уведомления можно будет получать уведомления при вызове ChangeState.
        /// </summary>


        public delegate void NotifyChangeEventHandler(string notifyinfo);

        public interface IObservable
        {

            void Attach(NotifyChangeEventHandler ohandler);

            void Detach(NotifyChangeEventHandler ohandler);

            void Notify(string name);

        }



        public abstract class AbstractObserver : IObservable
        {

            public void Register(NotifyChangeEventHandler handler)
            {

                this.Attach(handler);

            }

            public void UnRegister(NotifyChangeEventHandler handler)
            {

                this.Detach(handler);

            }

            public virtual void ChangeState()
            {

                this.Notify("ChangeState");

            }

            #region IObservable Members



            public void Attach(NotifyChangeEventHandler ohandler)
            {

                this.NotifyChanged += ohandler;

            }

            public void Detach(NotifyChangeEventHandler ohandler)
            {

                this.NotifyChanged -= ohandler;

            }

            public void Notify(string name)
            {

                if (this.NotifyChanged != null)

                    this.NotifyChanged(name);

            }

            #endregion

            #region INotifyChanged Members

            public event NotifyChangeEventHandler NotifyChanged;

            #endregion

        }



        public class Observer : AbstractObserver
        {

            public override void ChangeState()
            {

                // Какие-либо действия

                base.ChangeState();

            }

        }


        /// <summary>
        /// Шаблон итератора (Iterator)
        /// Этот шаблон представляет собой способ последовательно доступа к элементам набора. 
        /// Одним из примеров этого шаблона является интерфейс IEnumerable, предлагаемый корпорацией Майкрософт. 
        /// Разрешите мне показать данный шаблон на примере этого интерфейса.
        /// В приведенном коде можно видеть, что я реализовал интерфейс IEnumerable, который фактически соответствует блоку итератора. 
        /// Разработчик также может реализовать этот интерфейс самостоятельно, добавляя несколько методов, таких как First(), Last(), Next() и т. д. 
        /// Так как эти методы уже реализованы в качестве методов расширения интерфейса IEnumerable, я просто немного упростил его использование.
        /// </summary>


        public class Element
        {

            public string Name { get; set; }

        }



        public class Iterator : IEnumerable<Element>
        {

            public Element[] array;

            public Element this[int i]
            {

                get
                {

                    return array[i];

                }

            }



            #region IEnumerable<Element> Members

            public IEnumerator<Element> GetEnumerator()
            {

                foreach (Element arr in this.array)

                    yield return arr;

            }

            #endregion



            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {

                foreach (Element arr in this.array)

                    yield return arr;

            }

            #endregion

        }
        /// <summary>
        /// Всё о паттернах
        /// </summary>

    }
}
