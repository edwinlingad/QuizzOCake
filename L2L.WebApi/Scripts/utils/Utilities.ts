module edL {
    export class Utilites {
        prependNumber(numDecimalPlaces: number, num: number) : string {
            if (num < 10)
                return '0' + num;
            return num.toString();
        }
    }
} 