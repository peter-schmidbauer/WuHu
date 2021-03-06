import {Component, OnInit} from '@angular/core';
import {StatisticsService} from "../../api/statistics-service";
import {RanklistData} from "../../model/RanklistData";

@Component({
    selector: 'app-ranklist',
    templateUrl: './ranklist.component.html',
    styleUrls: ['./ranklist.component.sass']
})
export class RanklistComponent implements OnInit {

    private data : Array<RanklistData>;
    private errorMessage: string = "";
    private infoMessage: string = "";
    private loading: boolean = false;

    private sortedPlayers = [];

    constructor(private statsService: StatisticsService) { }

    sortByRank() {
        this.data.sort((a, b) => {
            if (!a.CurrentScore) return 1;
            if (!b.CurrentScore) return -1;
            return b.CurrentScore - a.CurrentScore
        });
    }

    getPlayers() {
        this.loading = true;
        this.statsService.statisticsGetRanklistData()
            .subscribe(
                res => {
                    this.data = res;
                    this.sortByRank();
                    this.errorMessage = "";
                    this.infoMessage = "";
                    this.loading = false;
                },
                error => {
                    this.errorMessage = "Verbindungsproblem";
                    this.infoMessage = "";
                    this.loading = false;
                }
            );
    }

    ngOnInit() {
        this.getPlayers();
    }
}
