using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class HandReceivedPayload : GameEventPayload
    {
        public List<SerializableCard> Cards { get; set; }

        public HandReceivedPayload() {}

        public HandReceivedPayload(HandReceived hr) : base(hr)
        {
            if (hr.Hand is KnownHand knownHand)
            {
                Cards = knownHand.Select(SerializableCard.From).ToList();
            } 
            else 
            {
                throw new NotSupportedException("cannot serialize hidden hands");
            }
        }
    }
}