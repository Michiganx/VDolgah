using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masha2
{
    public enum TicketType { StateTheaterTicket , PrivateTheaterTicket, PrivateConcertTicket}
    //прототип
    class Program
    {
        static void Main(string[] args)
        {
            Client.Instance.GetTicket(TicketType.PrivateConcertTicket).Info();
                Client.Instance.GetTicket(TicketType.PrivateTheaterTicket).Info();
                 Client.Instance.GetTicket(TicketType.StateTheaterTicket).Info();

            Console.Read();
        }

      
    }

   public  abstract class TicketFactory
    {
        public abstract Ticket CreateTicket(TicketType t);
    }

    public class StateTicketFactory : TicketFactory
    {
        public override Ticket CreateTicket(TicketType t)
        {
            return new StateTheaterTicket();
        }
    }

    public class PrivateTicketFactory : TicketFactory
    {

        public override Ticket CreateTicket(TicketType t)
        {
            switch (t)
            {
                case TicketType.PrivateTheaterTicket:
                    return new PrivateTheaterTicket();
                    
                case TicketType.PrivateConcertTicket:
                    return new PrivateConcertTicket();
                    
                default:
                    return null;
                    
            }
        }
    }

    public abstract class Ticket
    {
        public virtual void Info()
        {
            Console.WriteLine("this is a " + this.GetType().Name.ToString());
        }
    }

    public class StateTheaterTicket : Ticket
    {


    }
    public class PrivateTheaterTicket : Ticket
    {


    }
    public class PrivateConcertTicket : Ticket
    {


    }

    public class Client
    {
        TicketFactory f;
        private static Client instance = new Client();
        public static Client Instance
        {
            get
            {
                return instance;
            }
        }
        private  Client (){}

        public Ticket GetTicket(TicketType t)
        {
            switch (t)
            {
                case TicketType.StateTheaterTicket:
                    this.f = new StateTicketFactory();
                    break;
                case TicketType.PrivateTheaterTicket:
                case TicketType.PrivateConcertTicket:
                    this.f = new PrivateTicketFactory();
                    break;
                default:
                    return null;
                    
            }
            return this.f.CreateTicket(t);
        }
    }
}
