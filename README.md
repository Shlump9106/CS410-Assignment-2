# Changes to starter project:

# Jaryd

Used a dot product in conjunction with a ray to detect when the character model is looking into a mirror. Plays a soundclip and shoots particles out of the mirror when triggered. Added multiple extra mirrors throughout the level

# Ryan

Created a "cone of vision" for all enemies, which detects the player. When the player remains within range and unobstructed for a variable amount of time, a soundclip is played and different enemy behaviors are activated. Ghosts begin chasing the player after detection, and all ghosts chase the player after detection from a gargoyle.

# Clara

Setup sound effects for Benny Hill theme when being chased by ghosts as well as a Boo laugh sound from Mario when caught by a ghost. Also implemented sprinting with the shift key.

# Orion

Setup git repository, created particle effect for fire on the player character when detected by a gargoyle, and changed the waypoint script for ghosts to pick their next destination as a lerp vector between the player and the ghost. Ghosts will chase players when they spot them and end the game on collision.


# Linear Interpolation
- Ghost pathfinding to player uses linear interpolation to find where to set navmesh location to. Does not go directly to player and rather in between so that ghosts can be shaken off with tactical movement.

# Dot Product
- Mirrors exist across the level that have particle effects and sounds emitted when looked at, which is calculated with a dot product.

# Sound Effect
- Boo laugh from Mario plays when killed by ghost
- Benny Hill Theme plays while being chased
- Enemy detection sound plays when detected
- Mirrors emit whispers when looked at

# Particle Effect
- John Lemon is set on fire when detected by gargoyle
- Mirrors emit blood particles when looked at
