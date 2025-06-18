using System.Collections;
using ProjectSolamnia;

namespace ProjectSolamnia
{

    //karakterleri define eden sayfa
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Rank { get; set; }
        public StatusType Status { get; set; }

        public int? AssignedHoldingId { get; set; }
        //public Holding? AssignedHolding { get; set; }
        public string ActiveDuty { get; set; }

        public int Diplomacy { get; set; }
        public int Martial { get; set; }
        public int Stewardship { get; set; }
        public int Intrigue { get; set; }
        public int Learning { get; set; }
        public int Prowess { get; set; }

        
        



        /**
        Temel karakter constructoru. Bir karakter oluşturmak istendiğinde bu fonksiyon kullanılacak.
        Örnek: 
        Character hüsü = new Character(id,name,age,rank,status,assignedHoldingId,assignedHolding,activeDuty,diplomacy,martial,stewardship,intrigue,learning,prowess, new Trait[]{trait1, trait2});
        */
        public Character(int id, string name, int age, string rank, StatusType status,
                        int? assignedHoldingId, string activeDuty,
                        int diplomacy, int martial, int stewardship, int intrigue,
                        int learning, int prowess)
        {
            Id = id;
            Name = name;
            Age = age;
            Rank = rank;
            Status = status;
            AssignedHoldingId = assignedHoldingId;
            //AssignedHolding = assignedHolding;
            ActiveDuty = activeDuty;
            Diplomacy = diplomacy;
            Martial = martial;
            Stewardship = stewardship;
            Intrigue = intrigue;
            Learning = learning;
            Prowess = prowess;


        }



        /** 
            addTrait ( @param eklenecek trait objesi ) karaktere yeni bir trait ekler.
            removeTrait ( @param çıkarılacak trait objesi ) karakterin traitlerinden birini çıkarır.
        */
        //public void addTrait(Trait t)
        //{
        //    Traits.Add(t);
        //}
//
        //public void removeTrait(Trait t)
        //{
        //    Traits.Remove(t);
        //}
        


    }
}