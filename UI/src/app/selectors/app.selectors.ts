import { DatePipe } from "@angular/common";
import { createSelector } from "@ngrx/store";
import { colors } from "../common/app.colors";
import { BarItem, ChartItem } from "../components/app.component";
import { IAppState, IState } from "../reducers/app.reducer";

const appStateSelector = (state: IState) => state.app;

export const pieSelector = createSelector(
    appStateSelector,
    (state: IAppState) => {
        
        if (state.categories.length == 0)
            return null;

        return state.categories.reduce((result, c) => {
            const item = result.find(x => x.name == c.name);
            if (item) {
                item.value += c.total;
            } else {
                result.push({ name: c.name, value: c.total });
            }
            return result;
        }, [] as ChartItem[]);
    }
);
  
export const barSelector = createSelector(
    appStateSelector,
    (state: IAppState) => {

        if (state.categories.length == 0)
            return null;

        const barItems = getCategoriesFiltered(state)
            .reduce((result, curr) => {
                const hour = dateToBarItemName(curr.hour);
                const groupItem = result.find(x => x.name === hour);
                const chartItem = { name: curr.name, value: curr.total };

                if(!groupItem) {
                    result.push({ name: hour, series: [ chartItem ]});
                } else {
                    groupItem.series.push(chartItem);
                    groupItem.series.sort((a, b) => compare(a.name, b.name));
                }
                
                return result;
                }, [] as BarItem[]);

        const now = new Date();
        const categories: BarItem[] = [];
        now.setHours(now.getHours() - 23);

        for(let i = 23; i >= 0; i--) {
          const hour = dateToBarItemName(now);
          const barItem = barItems.find(x => x.name === hour);
          if(!barItem) {
            categories.push({name: hour, series: []});
          } else {
            categories.push(barItem);
          }
  
          now.setHours(now.getHours() + 1);
        }

        return categories;
    }
);

export const colorsSelector = createSelector(
    appStateSelector,    
    (state: IAppState) => {
        return state.categories.map((x, i) => {
            return { name: x.name, value: colors[i % colors.length] };
        });
    }
);

function getCategoriesFiltered(state: IAppState) {
    return state.selected 
        ? state.categories.filter(x => x.name == state.selected)
        : state.categories;
}

function compare(a: string, b: string) {
    if (a < b) { return -1; }
    if (a > b) { return 1; }
    return 0;
}

const datePipe = new DatePipe('en-US');

export function dateToBarItemName(value: string | Date) {
  return datePipe.transform(value, 'h a');
}
