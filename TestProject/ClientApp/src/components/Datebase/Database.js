import React, { useEffect } from 'react'
import { shallowEqual, useDispatch, useSelector } from 'react-redux';
import List from '../ListComponents/List'
import { createBackup, deleteBackup, getBackups, restore } from '../../actions/database';

const Database = () => {
    const dispatch = useDispatch();

    const { backups } = useSelector(state => ({
        backups: state.database.backups
    }), shallowEqual)

    useEffect(() => {
        dispatch(getBackups())
            .then(() => { })
            .catch(() => { });
    }, [dispatch])

    const createRecord = () => {
        dispatch(createBackup())
            .then(() => { alert("Success") })
            .catch(() => { })
    }

    const refreshRecords = () => {
        dispatch(getBackups())
            .then(() => { })
            .catch(() => { })
    }

    const deleteRecord = (item) => {
        dispatch(deleteBackup(item.backupName))
            .then(() => { })
            .catch(() => { })
    }

    const restoreDatabase = (item) => {
        dispatch(restore(item.backupName))
            .then(() => { alert("Success") })
            .catch(() => { })
    }

    const action = (item) => {
        return (
            <td>
                <button
                    onClick={() => { restoreDatabase(item) }}
                    style={{ marginRight: "3px"}}
                    className="btn btn-outline-warning btn-sm float-left">
                    <i className="bi-upload" />
                </button>
                <button
                    onClick={() => { deleteRecord(item) }}
                    className="btn btn-outline-danger btn-sm float-left">
                    <i className="bi-trash" />
                </button>
            </td>
        )
    }

    return (
        <List name="backups" records={backups} k="backupName" columns={['backupName']} createRecord={createRecord} refreshRecords={refreshRecords} action={action} />
    );
};

export default Database;