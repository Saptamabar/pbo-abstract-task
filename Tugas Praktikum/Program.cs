using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Tugas_Praktikum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int pilihRobot;
            string Nama;
            while (true)
            {
                Console.WriteLine("Selamat Datang!!");
                Console.Write("Masukan Nama Anda : ");
                Nama = Convert.ToString(Console.ReadLine());
                Console.WriteLine("Pilih Karakter anda : ");
                Console.WriteLine("1. Robot Penyerang");
                Console.WriteLine("2. Robot Bertahan");
                try
                {
                    pilihRobot = Convert.ToInt32(Console.ReadLine());
                    if (pilihRobot == 1 || pilihRobot == 2)
                    {
                        break;
                    }
                    else 
                    { 
                        Console.WriteLine("Pilihan Harus sesuai"); 
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch
                {
                    Console.WriteLine("Inputan Harus Berupa Angka");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.Clear();
            RobotTempur alpa = PilihRobot(pilihRobot,Nama);
            BosRobot sentaur = new BosRobot("Robot C67",200,15,30,20);
            while (true) 
            {
                Console.WriteLine("Player Stat :");
                alpa.CetakInformasi();
                Console.WriteLine("");
                Console.WriteLine("Enenmy Stat :");
                sentaur.CetakInformasi();
                if (alpa.Energi < 0)
                {
                    Console.WriteLine("Energi Anda habis anda kalah");
                    break;
                }
                else if (sentaur.Energi < 0)
                {
                    Console.WriteLine($"Energi {sentaur.Nama} habis anda menang");
                    sentaur.mati();
                    break;
                }
                Console.WriteLine("Pilih serangan");
                Console.WriteLine("1. Serang");
                Console.WriteLine("2. Gunakan Skill");
                int pilihan = Convert.ToInt32(Console.ReadLine());
                if (pilihan == 1) 
                {
                    alpa.Serang(sentaur);
                }
                else if (pilihan == 2) 
                {
                    int pilihan2;
                    Console.WriteLine("Pilih Skill");
                    Console.WriteLine($"1. Perbaikan ({alpa.cooldownPerbaikan} turn lagi)");
                    Console.WriteLine($"2. Serangan Listrik ({alpa.cooldownSeranganListrik} turn lagi)");
                    Console.WriteLine($"3. Serangan Plasma ({alpa.cooldownSeranganPlasma} turn lagi)");
                    Console.WriteLine($"4. Pertahanan Super ({alpa.cooldownPertahananSuper} turn lagi)");
                    Console.WriteLine($"5. Kemampuan Unix ({alpa.cooldownGunakanKemampuan} turn lagi)");
                    pilihan2 = Convert.ToInt32(Console.ReadLine());

                    switch (pilihan2) 
                    {
                        case 1:
                            alpa.Perbaikan(alpa);
                            break;
                        case 2:
                            alpa.SeranganListrik(sentaur);
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        case 3:
                            alpa.SeranganPlasma(sentaur);
                            break;
                        case 4:
                            alpa.PertahananSuper(alpa);
                            break;
                        case 5:
                            alpa.GunakanKemampuan(alpa);
                            break;

                    }
                }
            sentaur.diserang(alpa);
            Cooldown(alpa);
            Console.ReadKey();    
            Console.Clear();
            }
        }
        static void Cooldown(RobotTempur robot)
        {
            if (robot.cooldownPerbaikan > 0) {robot.cooldownPerbaikan -= 1; }
            if (robot.cooldownSeranganListrik > 0) {robot.cooldownSeranganListrik -= 1;}
            if (robot.cooldownSeranganPlasma > 0) {robot.cooldownSeranganPlasma -= 1; }
            if (robot.cooldownPertahananSuper > 0) {robot.cooldownPertahananSuper -= 1; robot.Armor -= 10; }
            if (robot.cooldownGunakanKemampuan > 0) {robot.cooldownGunakanKemampuan -= 1; robot.Serangan -= 10; }
        }

        static RobotTempur PilihRobot(int pilihan,string Nama)
        {
            if (pilihan == 1)
            {
                return new RobotTempur($"Robot {Nama}", 100, 20, 40);
            }
            else
            {
                return new RobotTempur($"Robot {Nama}",125,25,30);
            } 
        }
    }
    public abstract class Robot
    {
        public string Nama { get; set; }
        public int Energi { get; set; }
        public int Armor { get; set; }
        public int Serangan { get; set; }

        public Robot(string nama, int energi, int armor, int serangan) 
        {
            Nama = nama;
            Energi = energi;
            Armor = armor;
            Serangan = serangan;
        }

        virtual public void Serang(Robot target) { }

        virtual public void  GunakanKemampuan(Robot robot) { }

        virtual public void CetakInformasi() 
        {
            Console.WriteLine($"Nama    : {this.Nama}");
            Console.WriteLine($"Energi  : {this.Energi}");
            Console.WriteLine($"Armor   : {this.Armor}");
            Console.WriteLine($"Serangan: {this.Serangan}");
        }

    }
    class BosRobot : Robot
    {
        public int Pertahanan {get; set;}
        public BosRobot(string nama,int energi,int armor, int serangan, int pertahanan) : base(nama, energi, armor+pertahanan, serangan)
        {
            Pertahanan = pertahanan;
        }
        public void diserang(Robot penyerang)
        {
            int damage = this.Serangan - penyerang.Armor;
            if (damage > 0)
            {
                Console.WriteLine($"Bos {this.Nama} menyerang {penyerang.Nama}");
                Console.WriteLine($"{penyerang.Nama} kehilangan {damage} energi");
                penyerang.Energi -= damage;
            }
            else
            {
                Console.WriteLine($"Bos {this.Nama} menyerang {penyerang.Nama}, tetapi tidak menerima damage karena armor terlalu kuat!");
            }
        }

        public void mati()
        {
            Console.WriteLine($"Bos {this.Nama} telah dikalahkan");
        }
    }
    interface Kemampuan
    {
        void Perbaikan(Robot target);
        void SeranganListrik(Robot target);
        void SeranganPlasma(Robot target);
        void PertahananSuper(Robot target);
    }

    public class RobotTempur : Robot,Kemampuan
    {
        public int cooldownPerbaikan = 0;
        public int cooldownSeranganListrik = 0;
        public int cooldownSeranganPlasma = 0;
        public int cooldownPertahananSuper = 0;
        public int cooldownGunakanKemampuan = 0;
        public RobotTempur(string nama, int energi, int armor, int serangan) : base(nama,energi,armor,serangan) 
        { }

        override public void Serang(Robot target) 
        {
            int damage = this.Serangan - target.Armor;
            if (damage > 0)
            {
                Console.WriteLine($"{this.Nama} menyerang {target.Nama}");
                Console.WriteLine($"{target.Nama} kehilangan {damage} energi");
                target.Energi -= damage;
            }
            else
            {
                Console.WriteLine($"{this.Nama} menyerang {target.Nama}, tapi tidak menimbulkan damage karena armor terlalu kuat!");
            }
        }
        public void Perbaikan(Robot target)
        {
            if (this.cooldownPerbaikan == 0)
            {
                Energi += 20;
                Console.WriteLine("Perbaikan dilakukan");
                Console.WriteLine("Energi bertambah +20");
                this.cooldownPerbaikan = 3;
            }
            else
            {
                Console.WriteLine("Skill Sedang cooldown");
            }
        }
        public void SeranganListrik(Robot target)
        {
            if (this.cooldownSeranganListrik == 0)
            {
                int damage = this.Serangan + 20 - target.Armor;
                if (damage > 0)
                {
                    target.Energi -= damage;
                    Console.WriteLine("Serangan Listrik digunakan");
                    Console.WriteLine($"Robot {this.Nama} menyerang {target.Nama}");
                    Console.WriteLine($"{target.Nama} kehilangan {damage} energi");
                    Console.WriteLine($"{target.Nama} terkena Stun, giliran diabaikan");
                }
                else
                {
                    Console.WriteLine("Serangan Listrik digunakan, tetapi tidak menimbulkan damage karena armor terlalu kuat!");
                }
                this.cooldownSeranganListrik = 3;
            }
            else
            {
                Console.WriteLine("Skill Sedang cooldown");
            }
        }
        public void SeranganPlasma(Robot target)
        {
            if (this.cooldownSeranganPlasma == 0)
            {
                int damage = this.Serangan;
                if (damage > 0)
                {
                    target.Energi -= damage;
                    Console.WriteLine("Serangan Plasma digunakan");
                    Console.WriteLine($"{target.Nama} kehilangan {damage} energi");
                }
                else
                {
                    Console.WriteLine("Serangan Plasma digunakan, tetapi tidak menimbulkan damage karena armor terlalu kuat!");
                }
                this.cooldownSeranganPlasma = 3;
            }
            else
            {
                Console.WriteLine("Skill Sedang cooldown");
            }
        }
        public void PertahananSuper(Robot target)
        {
            if (this.cooldownPertahananSuper == 0)
            {
                target.Armor += 30;
                Console.WriteLine("Pertahanan Super digunakan");
                Console.WriteLine("Armor bertambah + 30 dalam 3 giliran");
                this.cooldownPertahananSuper = 3;
            }
            else
            {
                Console.WriteLine("Skill Sedang cooldown");
            }
        }
        public override void GunakanKemampuan(Robot robot)
        {
            if (this.cooldownGunakanKemampuan == 0)
            {
                robot.Serangan += 30;
                Console.WriteLine("Kemampuan Unik digunakan");
                Console.WriteLine("Serangan bertambah +30 dalam 3 giliran");
                this.cooldownGunakanKemampuan = 3;
            }
            else
            {
                Console.WriteLine("Skill Sedang cooldown");
            }
        }
    }

}
