using System.Linq;
using System;

using Data;

public class CampaignStore : ICampaignStore {

    public Data.Campaign GetCampaign(string campaignName) {
        return new Data.Campaign {
            missionNames = Campaign.FromString(campaignName).missionNames
        };
    }
}