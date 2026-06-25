import './Input.css';

export default function Input({ label, type = 'text', placeholder, value, onChange, error }) {
    return (
        <div className="input-group">
            {label && <label className="input-label">{label}</label>}
            <input
                type={type}
                className={`input-field ${error ? 'input-error' : ''}`}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
            />
            {error && <span className="input-error-text">{error}</span>}
        </div>
    );
}