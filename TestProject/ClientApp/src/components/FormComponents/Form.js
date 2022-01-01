import React from 'react';
import { useTranslation } from 'react-i18next';

import {
    Alert,
    Form,
    FormGroup,
} from 'reactstrap';

const FormComponent = ({ handleSubmit, children, message }) => {
    const { t } = useTranslation();

    return (
        <Form className="form" onSubmit={handleSubmit}>
            {children}
            {message && (
                <FormGroup>
                    <Alert color="danger" style={{ textAlign: 'center' }} className="mt-2">
                        {t(message)}
                    </Alert>
                </FormGroup>
            )}
        </Form>
    );
}

export default FormComponent;