using System;
using System.Collections.Generic;

namespace FootballWeb.Models
{
    // Mô hình chi tiết trận đấu
    public class MatchDetailViewModel
    {
        // Thông tin chung
        public int MatchId { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }

        // Các sự kiện chính của trận đấu
        public List<MatchEvent> Events { get; set; }

        // Đội hình thi đấu của hai đội
        public Lineup HomeLineup { get; set; }
        public Lineup AwayLineup { get; set; }

        // Thống kê trận đấu
        public MatchStatistics HomeStatistics { get; set; }
        public MatchStatistics AwayStatistics { get; set; }

        // Lịch sử đối đầu
        public List<PastMatch> PastMatches { get; set; }
    }

    // Mô hình sự kiện trong trận đấu (bàn thắng, thẻ vàng, thay người, v.v.)
    public class MatchEvent
    {
        public string Minute { get; set; } // Phút trong trận đấu
        public string Type { get; set; } // Loại sự kiện (goal, yellow_card, substitution, v.v.)
        public string Player { get; set; } // Cầu thủ tham gia
        public string Description { get; set; } // Mô tả chi tiết sự kiện
    }

    // Mô hình cầu thủ
    public class Player
    {
        public int Number { get; set; } // Số áo cầu thủ
        public string Name { get; set; } // Tên cầu thủ
        public string Position { get; set; } // Vị trí cầu thủ (GK, DF, MF, FW)
    }

    // Mô hình đội hình thi đấu
    public class Lineup
    {
        public string Formation { get; set; } // Sơ đồ đội hình (ví dụ 4-4-2)
        public List<Player> StartingPlayers { get; set; } // Danh sách cầu thủ đá chính
        public List<Player> Substitutes { get; set; } // Danh sách cầu thủ dự bị
    }

    // Mô hình thống kê trận đấu
    public class MatchStatistics
    {
        public string Possession { get; set; } // Tỷ lệ cầm bóng (ví dụ "55%")
        public int Shots { get; set; } // Số lần sút bóng
        public int ShotsOnTarget { get; set; } // Số lần sút trúng đích
        public int Corners { get; set; } // Số quả phạt góc
        public int YellowCards { get; set; } // Số thẻ vàng
        public int RedCards { get; set; } // Số thẻ đỏ
    }

    // Mô hình lịch sử đối đầu giữa hai đội
    public class PastMatch
    {
        public DateTime Date { get; set; } // Ngày thi đấu
        public string HomeTeamName { get; set; } // Tên đội chủ nhà
        public string AwayTeamName { get; set; } // Tên đội khách
        public string Result { get; set; } // Kết quả trận đấu (ví dụ: "2-1", "Hòa", v.v.)
    }


}
