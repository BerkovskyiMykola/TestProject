import React from 'react';
import Form from "react-validation/build/form";
import CheckButton from "react-validation/build/button";
import { useTranslation } from 'react-i18next';

const FormComponent = ({ setForm, handleSubmit, children, message, setCheckBtn }) => {
    const { t } = useTranslation();

    return (
        <Form
            onSubmit={handleSubmit}
            ref={setForm}
        >
            {children}
            {message && (
                <div className="form-group">
                    <div className="alert alert-danger" role="alert">
                        {t(message)}
                    </div>
                </div>
            )}
            <CheckButton
                style={{ display: "none" }}
                ref={setCheckBtn}
            />
        </Form>
    );
}

export default FormComponent;