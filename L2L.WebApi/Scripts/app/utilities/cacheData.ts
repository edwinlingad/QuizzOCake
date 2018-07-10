

class DataCache {
    private maxSize: number;
    private data: any[]

    constructor(maxSize: number) {
        this.maxSize = maxSize;
        this.data = new Array<any>();
    }

    public addEntry(entry: any) {
        this.data.push(entry);
        if (this.data.length > 5)
            this.data.splice(1, 1);
    }

    public getEntry(id: number): any {
        for (var i = 0; i < this.data.length; i++) {
            if (this.data[i].id == id)
                return this.data[i];
        }
        return undefined;
    }

    public deleteEntry(id: number) {
        for (var i = 0; i < this.data.length; i++) {
            if (this.data[i].id == id) {
                this.data.splice(i, 1);
            }
        }
    }

    public getLastEntry(): any {
        if (this.data.length > 0)
            return this.data[this.data.length - 1];
        return undefined;
    }
} 