using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;
using static Program;

internal class Program
{
    public class BankTransferConfig
    {
        public Konfig konfig;
        private const string filepath = @"bank_transfer_config.json";

        public BankTransferConfig()
        {
            try
            {
                ReadKonfigFile();
            }
            catch
            {
                SetDefault();
                WriteKonfigFile();
            }
        }
        public void ReadKonfigFile()
        {
            string hasilBaca = File.ReadAllText(filepath);
            konfig = JsonSerializer.Deserialize<Konfig>(hasilBaca);
        }
        public void WriteKonfigFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string tulisan = JsonSerializer.Serialize(konfig, options);
            File.WriteAllText(filepath, tulisan);
        }
        public void SetDefault()
        {
            konfig = new Konfig();
            konfig.lang = "en";
            Transfer transfer = new Transfer(25000000, 6500, 15000);
            konfig.transfer = transfer;
            List<string> isi = new List<string>();
            isi.Add("RTO (real-time)");
            isi.Add("SKN");
            isi.Add("RTGS");
            isi.Add("BI FAST");
            konfig.methods = isi;
            Confirmation konfirm = new Confirmation();
            konfirm.en = "Yes";
            konfirm.id = "ya";
            konfig.confirmation = konfirm;
        }
    }
    public class Konfig
    {
        public string lang{ get; set; }
        public Transfer transfer { get; set; }
        public List<string> methods { get; set; }
        public Confirmation confirmation { get; set; }
        public Konfig() { }
        public Konfig(string lang, Transfer transfer, List<string> methods, Confirmation confirmation)
        {
            this.lang = lang;
            this.transfer = transfer;
            this.methods = methods;
            this.confirmation = confirmation;
        }
    }
    public class Transfer
    {
        public double threshold { get; set; }
        public double low_fee { get; set; }
        public double high_fee { get; set; }
        public Transfer() { }
        public Transfer(double threshold, double low_fee, double high_fee)
        {
            this.threshold = threshold;
            this.low_fee = low_fee;
            this.high_fee = high_fee;
        }
    }
    public class Confirmation
    {
        public string en { get; set; }
        public string id { get; set; }
        public Confirmation() { }
        public Confirmation(string en, string id)
        {
            this.en = en;
            this.id = id;
        }
    }
    private static void Main(string[] args)
    {
        BankTransferConfig bank = new BankTransferConfig();
        if(bank.konfig.lang == "en")
        {
            Console.WriteLine("Please insert the amount of money to transfer: ");
        }
        else
        {
            Console.WriteLine("Masukkan jumlah uang yang akan di - transfer: ");
        }
        double uangTF = Convert.ToInt32(Console.ReadLine());
        double biayaTF = 0;
        if(uangTF <= bank.konfig.transfer.threshold)
        {
            biayaTF = bank.konfig.transfer.low_fee;
        }
        else
        {
            biayaTF = bank.konfig.transfer.high_fee;
        }
        if (bank.konfig.lang == "en")
        {
            Console.WriteLine("Transfer fee = " + biayaTF);
            Console.WriteLine("Total amount = " + (uangTF + biayaTF));
        }
        else
        {
            Console.WriteLine("Biaya transfer = " + biayaTF);
            Console.WriteLine("Total biaya = " + (uangTF + biayaTF));
        }
        Console.WriteLine("1. " + bank.konfig.methods[0]);
        Console.WriteLine("2. " + bank.konfig.methods[1]);
        Console.WriteLine("3. " + bank.konfig.methods[2]);
        Console.WriteLine("4. " + bank.konfig.methods[3]);

        if (bank.konfig.lang == "en")
        {
            Console.WriteLine("EN => Please type " + bank.konfig.confirmation.en + " to confirm the transaction:");
        }
        else
        {
            Console.WriteLine("ID => Ketik "+ bank.konfig.confirmation.id + " untuk mengkonfirmasi transaksi:");
        }
        string jawaban = Console.ReadLine();
        if(jawaban == bank.konfig.confirmation.en)
        {
            Console.WriteLine("The transfer is completed");
        }else if(jawaban == bank.konfig.confirmation.id)
        {
            Console.WriteLine("Proses transfer berhasil");
        }
        else
        {
            if(bank.konfig.lang == "EN")
            {
                Console.WriteLine("Transfer is cancelled");
            }
            else
            {
                Console.WriteLine("Transfer dibatalkan");
            }
        }
    }
}