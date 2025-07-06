export default function Profile({ user }) {
    return (
      <div>
        <h2>Hello, {user.Name}</h2>
        <p>Email: {user.Email}</p>
      </div>
    );
  }
  