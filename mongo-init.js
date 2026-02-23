print("🔍 Checking if replica set is already initialized...");

try {
  const status = rs.status();
  if (status.ok) {
    print("✅ Replica set 'rs0' is already active.");
  }
} catch (e) {
  print("🔄 Initializing replica set 'rs0'...");
  const config = {
    _id: "rs0",
    members: [
      { _id: 0, host: "gym-mongodb:27017" }
    ]
  };
  const result = rs.initiate(config);
  if (result.ok) {
    print("✅ Replica set initialized successfully!");
  } else {
    print("❌ Failed to initialize replica set:", result);
  }
}