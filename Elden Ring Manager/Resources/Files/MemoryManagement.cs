using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using Elden_Ring_Manager;
using Elden_Ring_Manager.Resources.Files;
using Swed64;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class MemoryManagement
{
    private Swed swed;
    private long moduleBase;
    private ulong WrldCharMANBASE;
    private ulong WrldCharMAN;

    private ulong GameDataManBASE;
    private ulong GameDataMan;
    public MemoryManagement()
    {
        string processName = "eldenring";
        swed = new Swed(processName);
        moduleBase = swed.GetModuleBase("eldenring.exe");
        WrldCharMANBASE = (ulong)moduleBase + 0x3D65F88;
        WrldCharMAN = BitConverter.ToUInt64(swed.ReadBytes((nint)WrldCharMANBASE, 8), 0);
        GameDataManBASE = (ulong)moduleBase + 0x3D5DF38;
        GameDataMan = BitConverter.ToUInt64(swed.ReadBytes((nint)GameDataManBASE, 8), 0);
    }

    public void Dispose()
    {
        swed = null;
        moduleBase = 0;
        WrldCharMANBASE = 0;
        WrldCharMAN = 0;
        GameDataManBASE = 0;
        GameDataMan = 0;
        GC.SuppressFinalize(this);
    }
    ~MemoryManagement()
    {
        Dispose();
    }


    public string getCharacterName()
    {
        nint address1 = swed.ReadPointer((nint)GameDataMan, 0x08);
        byte[] rawBytes = swed.ReadBytes(address1 + 0x9C, 64);
        string decodedName = Encoding.Unicode.GetString(rawBytes).TrimEnd('\0');
        //Console.WriteLine(decodedName);
        return decodedName;
    }
    public int getLevel()
    {
        nint address1 = swed.ReadPointer((nint)GameDataMan, 0x08);
        int lvl = swed.ReadInt(address1, 0x68);
        return lvl;
    }

    public static int hpScrollBarValue;
    public static int fpScrollBarValue;
    public static int SpScrollBarValue;
    public static int animSpeedScrollbarValue;
    public static bool freezeHP;
    public static bool freezeFP;
    public async Task<int> GetHP(bool write = false) //writable
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int HP = swed.ReadInt(address4, 0x138);
        if (write)
        {
            swed.WriteInt(address4, 0x138, hpScrollBarValue);
        }
        return HP;
    }
    public async void FreezeHP()
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int HP = swed.ReadInt(address4, 0x138);
        while (freezeHP)
        {
            swed.WriteInt(address4, 0x138, hpScrollBarValue);
            await Task.Delay(100);
            if (!freezeHP)
                break;
        }
    }

    public int getMaxHP()
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int MAXHP = swed.ReadInt(address4, 0x13C);
        return MAXHP;
    }

    public async Task<int> GetFP(bool write = false) //writable
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int FP = swed.ReadInt(address4, 0x148);
        if (write)
        {
            swed.WriteInt(address4, 0x148, fpScrollBarValue);
        }
        return FP;
    }
    public async void FreezeFP()
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int FP = swed.ReadInt(address4, 0x148);
        while (freezeFP)
        {
            swed.WriteInt(address4, 0x148, fpScrollBarValue);
            await Task.Delay(100);
            if (!freezeFP)
                break;
        }
    }
    public int getMaxFP()
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int MAXFP = swed.ReadInt(address4, 0x14C);
        return MAXFP;
    }

    public int GetStamina(bool write = false) //writable
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int Stamina = swed.ReadInt(address4, 0x154);
        if (write)
        {
            swed.WriteInt(address4, 0x154, SpScrollBarValue);
        }
        return Stamina;
    }
    public int getMaxStamina()
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x0);
        int MaxStamina = swed.ReadInt(address4, 0x158);
        return MaxStamina;
    }

    public int GetRunes(bool write = false, int value = 0) //writable
    {
        nint address1 = swed.ReadPointer((nint)GameDataMan, 0x08);
        int runes = swed.ReadInt(address1, 0x6C);
        if (write)
        {
            swed.WriteInt(address1, 0x6C, value);
        }
        return runes;
    }

    public bool GetRuneArcStatus(bool off = false, bool on = false)
    {
        nint address1 = swed.ReadPointer((nint)GameDataMan, 0x8);
        bool runeArcStatus = swed.ReadBool(address1, 0xFF);
        byte[] isOn = {1};
        byte[] isOff = {0};
        if (runeArcStatus && off)
        {
            swed.WriteBytes(address1, 0xFF, isOff);
        }
        else if(!runeArcStatus && on)
        {
            swed.WriteBool(address1, 0xFF, on);
        }
        return runeArcStatus;
    }

    public int[] GetAllStats()
    {
        nint PTR1 = swed.ReadPointer((nint)GameDataMan, 0x8);
        int Vigor = swed.ReadInt(PTR1, 0x3C);
        int Mind = swed.ReadInt(PTR1, 0x40);
        int Endurance = swed.ReadInt(PTR1, 0x44);
        int Strength = swed.ReadInt(PTR1, 0x48);
        int Dexterity = swed.ReadInt(PTR1, 0x4C);
        int Intelligence = swed.ReadInt(PTR1, 0x50);
        int Faith = swed.ReadInt(PTR1, 0x54);
        int Arcane = swed.ReadInt(PTR1, 0x58);
        return new int[] { Vigor, Mind, Endurance, Strength, Dexterity, Intelligence, Faith, Arcane };
    }
    public void SetStatus(int offset, int value)
    {
        nint PTR1 = swed.ReadPointer((nint)GameDataMan, 0x8);
        swed.WriteInt(PTR1, offset, value);
    }


    public static bool noDead = false;
    public static bool noDamage = false;
    public static bool noFPcons = false;
    public static bool noSPcons = false;
    public static bool noAttack = false;
    public static bool noMove = false;
    public static bool noUpdate = false;

    public bool NoDead(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit0 = (originalByte & (1 << 0)) != 0 ? 1 : 0;
            if (bit0 == 0)
            {
                noDead = false;
                return false;
            }
            else
            {
                noDead = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noDead)
            {
                modifiedByte = (byte)(originalByte | (1 << 0));
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 0));
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x19B, new byte[] { modifiedByte });
            noDead = !noDead;
        }
        return false;
    }
    

    public bool NoDamage(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit1 = (originalByte & (1 << 1)) != 0 ? 1 : 0;
            if (bit1 == 0)
            {
                noDamage = false;
                return false;
            }
            else
            {
                noDamage = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noDamage)
            {
                modifiedByte = (byte)(originalByte | (1 << 1)); // set bit 4 to 1 (enable No Attack)
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 1)); // clear bit 4 to 0 (disable No Attack)
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x19B, new byte[] { modifiedByte });
            noDamage = !noDamage;
        }
        return false;
    }

    public bool NoFpConsumption(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit2 = (originalByte & (1 << 2)) != 0 ? 1 : 0;
            if (bit2 == 0)
            {
                noFPcons = false;
                return false;
            }
            else
            {
                noFPcons = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noFPcons)
            {
                modifiedByte = (byte)(originalByte | (1 << 2)); // set bit 4 to 1 (enable No Attack)
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 2)); // clear bit 4 to 0 (disable No Attack)
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x19B, new byte[] { modifiedByte });
            noFPcons = !noFPcons;
        }
        return false;
    }

    public bool NoSpConsumption(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit3 = (originalByte & (1 << 3)) != 0 ? 1 : 0;
            if (bit3 == 0)
            {
                noSPcons = false;
                return false;
            }
            else
            {
                noSPcons = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0, 0x190, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x19B, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noSPcons)
            {
                modifiedByte = (byte)(originalByte | (1 << 3));
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 3));
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x19B, new byte[] { modifiedByte });
            noSPcons = !noSPcons;
        }
        return false;
    }

    public bool NoAttack(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x530, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit4 = (originalByte & (1 << 4)) != 0 ? 1 : 0;
            if (bit4 == 0)
            {
                noAttack = false;
                return false;
            }
            else
            {
                noAttack = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x530, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noAttack)
            {
                modifiedByte = (byte)(originalByte | (1 << 4));
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 4));
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x530, new byte[] { modifiedByte });
            noAttack = !noAttack;
        }
        return false;
    }

    public bool NoMove(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x530, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit5 = (originalByte & (1 << 5)) != 0 ? 1 : 0;
            if (bit5 == 0)
            {
                noMove = false;
                return false;
            }
            else
            {
                noMove = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x530, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noMove)
            {
                modifiedByte = (byte)(originalByte | (1 << 5));
                Console.WriteLine("Set bit 4 to 1");
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 5));
                Console.WriteLine("Set bit 4 to 0");
            }
            swed.WriteBytes(address1, 0x530, new byte[] { modifiedByte });
            noMove = !noMove;
        }
        return false;
    }

    public bool NoUpdate(bool check = false)
    {
        if (check)
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x531, 1);
            if (data == null || data.Length < 1) return false;
            byte originalByte = data[0];
            int bit0 = (originalByte & (1 << 0)) != 0 ? 1 : 0;
            if (bit0 == 0)
            {
                noUpdate = false;
                return false;
            }
            else
            {
                noUpdate = true;
                return true;
            }
        }
        else
        {
            nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8, 0x0);
            byte[] data = swed.ReadBytes(address1, 0x531, 1);

            if (data == null || data.Length < 1) return false;

            byte originalByte = data[0];
            byte modifiedByte;

            if (!noUpdate)
            {
                modifiedByte = (byte)(originalByte | (1 << 0));
            }
            else
            {
                modifiedByte = (byte)(originalByte & ~(1 << 0));
            }
            swed.WriteBytes(address1, 0x531, new byte[] { modifiedByte });
            noUpdate = !noUpdate;
        }
        return false;
    }

    public int GetAnimationSpeed(bool write = false) //writable
    {
        nint address1 = swed.ReadPointer((nint)WrldCharMAN, 0x10EF8);
        nint address2 = swed.ReadPointer(address1, 0x0);
        nint address3 = swed.ReadPointer(address2, 0x190);
        nint address4 = swed.ReadPointer(address3, 0x28);
        float animationSpeed = swed.ReadFloat(address4, 0x17C8);
        if (write)
        {
            swed.WriteFloat(address4, 0x17C8, animSpeedScrollbarValue);
        }
        return (int)animationSpeed;
    }

}
