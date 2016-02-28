﻿/// <reference path="../_all.ts" />
module dockyard.interfaces {

    // TODO: Do we really need all these interfaces, since we have model type safe classes?

    export interface ICriteriaDTO {
        id: string;
        isTempId: boolean;
        activities: Array<IActivityDTO>;
        conditions: Array<ICondition>;
        executionType: model.CriteriaExecutionType;
    }

    export interface IActivityDTO {
        id: string;
        parentRouteNodeId: string;
        label?: string;
        crateStorage: model.CrateStorage;
        configurationControls: model.ControlsList;
        activityTemplate: model.ActivityTemplate;
        childrenActivities: Array<IActivityDTO>;
        ordering: number;
    }

    export interface IActivityCategoryDTO {
        name: string;
        activities: Array<IActivityTemplateVM>
    }

    export interface ICondition {
        field: string;
        operator: string;
        value: string;
        valueError: string;
    }

    export interface IField {
        key: string;
        name: string;
    }

    export interface IFileDescriptionDTO {
        id: number;
        originalFileName: string;
    }

    export interface IDocuSignTemplate {
        id: number;
        name: string;
        description: string;
    }

    export interface IDocuSignExternalEvent {
        id: number;
        name: string;
        description: string;
    }
}
