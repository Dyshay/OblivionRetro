using AivyData.Model;
using AivyDofus.Handler;
using AivyDofus.Model.Characters.Skills;
using AivyDomain.Callback.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Proxy.Handlers.Customs.World.Character
{

    [ProxyHandler(ProtocolName = "JS")]
    public class CharacterSkillsMessageHandler : AbstractMessageHandler
    {
        public CharacterSkillsMessageHandler(AbstractClientReceiveCallback callback, string package, Account account) : base(callback, package, account)
        {
        }

        public override bool IsForwardingData => true;

        public override async Task Handle()
        {
            int JobId;
            string[] InfoArraySkill;
            int IdSkill;
            byte MaxQuantity, MinQuantity;
            float GatheringTime;
            //foreach (string data in _package.Substring(3).Split('|'))
            //{
            //    JobId = short.Parse(data.Split(';')[0]);
            //    //Job  = prmClient.Account.Game.Character.Job.Find(x => x.id == JobId);

            //    // si Job non associé au perso alors add to character
            //    //if (Job == null)
            //    //{
            //    //    oficio = new Job(JobId);
            //    //    personaje.oficios.Add(oficio);
            //    //}

            //    foreach (string datos_skill in data.Split(';')[1].Split(','))
            //    {
            //        InfoArraySkill = datos_skill.Split('~');
            //        IdSkill = short.Parse(InfoArraySkill[0]);
            //        MinQuantity = byte.Parse(InfoArraySkill[1]);
            //        MaxQuantity = byte.Parse(InfoArraySkill[2]);
            //        GatheringTime = float.Parse(InfoArraySkill[4]);

            //        _account.Character.Skills.Add(new Skill(IdSkill, MaxQuantity, MinQuantity, GatheringTime));
            //    }
            //}
        }
    }
}
