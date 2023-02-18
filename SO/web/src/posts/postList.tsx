import * as React from 'react';
import { useEffect } from 'react';
import { Api } from '../apiClient/Api';
import { PostListDto } from '../apiClient/data-contracts';
import toast from 'react-hot-toast';
import { GridDemoData, useDemoData } from '@mui/x-data-grid-generator';
import { GridRowsProp } from '@mui/x-data-grid/models/gridRows';
import { GridSelectionModel } from '@mui/x-data-grid/models/gridSelectionModel';
import { DataGrid } from '@mui/x-data-grid';
import { GridColDef } from '@mui/x-data-grid-pro';
import CheckIcon from '@mui/icons-material/Check';
import CloseIcon from '@mui/icons-material/Close';
import { format } from 'date-fns';

function loadServerRows(page: number, pageSize: number): Promise<any> {
    return new Promise(async (resolve) => {
        const api = new Api({
            baseUrl: "http://localhost:5000"
        });
        
        const offset = page * pageSize;
        const posts = await api.postList({ Offset: offset, Limit: pageSize });
        if (posts.ok) {
            console.log(posts.data)
            resolve(posts.data ?? undefined);
        } else {
            toast("Cannot get posts");
        }
    });
}

export default function PostList() {
    const pageSize = 5;
     
    const [rowCount, setRowCount] = React.useState(10);
    const [page, setPage] = React.useState(0);
    const [rows, setRows] = React.useState<GridRowsProp>([]);
    const [loading, setLoading] = React.useState(false);

    function renderStatus(params: any) {
        return params.row.isClosed ? <CloseIcon /> : <CheckIcon />;
    }

    function renderDate(date: any) {
        return format(new Date(date ?? ""), 'yyyy-MM-dd');
    }

    function renderNumberWithSpace(number: any) {
        return parseInt(number).toLocaleString('en-US').replace(/,/g," ",);
    }

    const columns: GridColDef[] = [
        { field: 'title', headerName: 'Title', width: 370 },
        { field: 'creationDate', headerName: 'Create date', width: 100, renderCell: (params: any) => renderDate(params.row.creationDate) },
        { field: 'viewCount', headerName: 'Views', width: 90, renderCell: (params: any) => renderNumberWithSpace(params.row.viewCount) },
        { field: 'tags', headerName: 'Tags', width: 130 },
        { field: 'userName', headerName: 'User', width: 130 },
        { field: 'commentCount', headerName: 'Comments', width: 100, renderCell: (params: any) => renderNumberWithSpace(params.row.commentCount) },
        { field: 'isClosed', headerName: 'Active', width: 60, renderCell: (params: any) => renderStatus(params) }
    ];

    React.useEffect(() => {
        let active = true;
        (async () => {
            setLoading(true);
            const response = await loadServerRows(page, pageSize);

            if (!active)
                return;
            
            setRowCount(response.count);
            setRows(response.posts);
            setLoading(false);
        })();

        return () => {
            active = false;
        };
    }, [page]);

    return (
        <div style={{ height: 400, width: '100%' }}>
            <DataGrid
                rows={rows}
                columns={columns}
                pagination
                pageSize={pageSize}
                rowsPerPageOptions={[5]}
                rowCount={rowCount}
                paginationMode="server"
                onPageChange={(newPage) => {
                    setPage(newPage);
                }}
                loading={loading}
            />
        </div>
    );
}
