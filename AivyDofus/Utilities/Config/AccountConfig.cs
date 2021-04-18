using System.IO;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Utilities.Config
{
    public class AccountConfig
    {
        public string accountUsername { get; set; } = string.Empty;
        public string accountPassword { get; set; } = string.Empty;

        public AccountConfig(string _accountUsername, string _accountPassword)
        {
            accountUsername = _accountUsername;
            accountPassword = _accountPassword;
        }

        public void SaveAccount(BinaryWriter bw)
        {
            bw.Write(accountUsername);
            bw.Write(accountPassword);
        }

        public static AccountConfig LoadAcccount(BinaryReader br)
        {
            try
            {
                return new AccountConfig(br.ReadString(), br.ReadString());
            }
            catch
            {
                return null;
            }
        }
    }
}
