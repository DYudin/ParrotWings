import { Pipe, PipeTransform } from "@angular/core";
@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {
    transform(array: Array<any>, orderField: string, orderType: boolean): Array<string> {
        array.sort((a: any, b: any) => {
            if (a[orderField] == null || a[orderField].isUndefined) return orderType ? 0 - b[orderField] : b[orderField] - 0;
            if (b[orderField] == null || b[orderField].isUndefined) return orderType ? a[orderField] - 0 : b[orderField] - 0;
            return orderType ? a[orderField] - b[orderField] : b[orderField] - a[orderField];
        });
        return array;
    }
}