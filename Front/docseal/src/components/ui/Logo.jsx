import logo from '/src/assets/logo.png';
import './Logo.css';

export default function Logo({ size = 'md', position = 'right' }) {
    return (
        <div className={`logo logo-${size} logo-${position}`}>
            <img src={logo} alt="DOCseal" className="logo-img" />
            <span className="logo-text">DOCseal</span>
        </div>
    );
}