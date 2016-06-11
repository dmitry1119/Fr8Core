﻿module dockyard.controllers {
    'use strict';

    export interface IReportFactListScope extends ng.IScope {
        filter: any;
        query: model.HistoryQueryDTO;
        promise: ng.IPromise<model.HistoryResultDTO<model.FactDTO>>;
        result: model.HistoryResultDTO<model.FactDTO>;
        getHistory: () => void;
        removeFilter: () => void;
        expandItem: (historyItem: model.HistoryItemDTO) => void;
        orderBy: string;
        selected: any;
    }

    class ReportFactController {

        public static $inject = [
            '$scope',
            'ReportService'
        ];

        constructor(private $scope: IReportFactListScope, private ReportService: services.IReportService) {

            $scope.selected = [];

            $scope.query = new model.HistoryQueryDTO();
            $scope.query.itemPerPage = 10;
            $scope.query.page = 1;
            $scope.orderBy = "-createdDate";
            $scope.query.isCurrentUser = true;


            $scope.filter = {
                options: {
                    debounce: 500
                }
            };

            $scope.getHistory = <() => void>angular.bind(this, this.getHistory);
            $scope.removeFilter = <() => void>angular.bind(this, this.removeFilter);
            $scope.expandItem = <(historyItem: model.HistoryItemDTO) => void>angular.bind(this, this.expandItem);

            $scope.$watch('query.filter', (newValue, oldValue) => {
                var bookmark: number = 1;
                if (!oldValue) {
                    bookmark = $scope.query.page;
                }
                if (newValue !== oldValue) {
                    $scope.query.page = 1;
                }
                if (!newValue) {
                    $scope.query.page = bookmark;
                }

                this.getHistory();
            });
        }

        private expandItem(historyItem: model.HistoryItemDTO) {
            if ((<any>historyItem).$isExpanded) {
                (<any>historyItem).$isExpanded = false;
            } else {
                (<any>historyItem).$isExpanded = true;
            }
        }

        private removeFilter() {
            this.$scope.query.filter = null;
            this.$scope.filter.showFilter = false;
            this.getHistory();
        }

        private getHistory() {
            if (this.$scope.orderBy && this.$scope.orderBy.charAt(0) === '-') {
                this.$scope.query.isDescending = true;
            } else {
                this.$scope.query.isDescending = false;
            }
            this.$scope.promise = this.ReportService.getFactsByQuery(this.$scope.query).$promise;
            this.$scope.promise.then((data: model.HistoryResultDTO<model.FactDTO>) => {
                this.$scope.result = data;
            });
        }
    }

    app.controller('ReportFactController', ReportFactController);
}