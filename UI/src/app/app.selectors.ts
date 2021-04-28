import { createSelector } from "@ngrx/store";
import { BarItem } from "./app.component";
import { dateToBarItemName } from "./app.helpers";
import { IAppState, IState } from "./app.reducer";

export const pieSelector = createSelector(
    (state: IState) => state.app,
    (state: IAppState) => state.categories
);
  
export const barSelector = createSelector(
    (state: IState) => state.app,
    (state: IAppState) => {
        const barItems = getCategoriesByHourFiltered(state);
        const now = new Date();
        const categoriesByHour: BarItem[] = [];
        now.setHours(now.getHours() - 23);
        for(let i = 23; i >= 0; i--) {
          const hour = dateToBarItemName(now);
          const barItem = barItems.find(x => x.name === hour);
          if(!barItem) {
            categoriesByHour.push({name: hour, series: []});
          } else {
            categoriesByHour.push(barItem);
          }
  
          now.setHours(now.getHours() + 1);
        }
        return categoriesByHour;
    }
);

function getCategoriesByHourFiltered(state: IAppState) {
    if(!state.selected)
        return state.categoriesByHour;
    
    return state.categoriesByHour.reduce((result, group) => {
        const category = group.series.find(x => x.name === state.selected);
        if(!!category) {
            result.push({ name: group.name, series: [category ]});
        }
        return result;
    }, [] as BarItem[]);
}
