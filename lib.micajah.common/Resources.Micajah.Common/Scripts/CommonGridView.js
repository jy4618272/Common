function Cgv_Select(evnt, func) {
    var elem = (evnt.target || evnt.srcElement);
    if (func && elem && (elem.tagName == 'TD' || elem.tagName == 'TR')) eval(func);
}

function Cgv_InsertChildToTableRow(sourceElementId, destTableId, rowIndex) {
    var sourceElement = document.getElementById(sourceElementId);
    var destTable = document.getElementById(destTableId);
    if (sourceElement && destTable) {
        var colSpan = -1;
        if (rowIndex > 0) {
            var previousRow = destTable.rows[rowIndex - 1];
            for (var i = 0; i < previousRow.cells.length; i++) {
                var s = parseInt(previousRow.cells[i].colSpan);
                if (!isNaN(s))
                    colSpan += s;
                else
                    colSpan++;
            }
        }
        var row = destTable.insertRow(rowIndex);
        var cell = document.createElement('TD');
        cell.className = 'Cgv_Crbo';
        row.appendChild(cell);
        cell = document.createElement('TD');
        cell.className = 'Cgv_Ci';
        if (colSpan > 1) cell.colSpan = colSpan;
        cell.appendChild(sourceElement);
        row.appendChild(cell);
    }
}
