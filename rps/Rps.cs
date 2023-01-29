using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace rps
{
    // The Rock Paper Scissors Match uses the following model:
    //
    // int Id               : db record id
    //
    // string Rules         : a JSON string representing the possible choices and the corresponding victory conditions
    //                      : the app will pick randomly from these rules and evaulate the player choice against the victory condition
    //                      : the default rules contain traditional "rock/paper/scissors" but can be changed to different rules.
    //                      : The string match for victory conditions allows for more complex rule sets like
    //                      : rock/paper/scissors/spock/lizard (http://www.samkass.com/theories/RPSSL.html)
    //
    // string PlayerName    : name of the player that is challenging the app to RPS
    //
    // string PlayerThrow   : the choice that the player "throws"
    //
    // string Result        : name of the victor (or a draw)

    public class RpsMatch
    {
        public int Id { get; set; }

        [DefaultValue("{\"rock\":\"paper\",\"paper\":\"scissors\",\"scissors\":\"rock\"}")]
        public string Rules { get; set; } // = {"rock":"paper","paper":"scissors","scissors":"rock"};

        public string? PlayerName { get; set; }
        public string PlayerThrow { get; set; }
        public string AppThrow { get; set; }
        public string Winner { get; set; } = "Draw";
    }

    // The RpsRule model is deserialized from the Rules in the RpsMatch model
    public class RpsRule
    {
        public string Choice { get; set; }
        public string[] VictoryCondition { get; set; }
    }

}
