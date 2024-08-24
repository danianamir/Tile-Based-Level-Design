# Tile-Based-Level-Design
# Reinforcement Learning for Tile-Based Game Level Generation

This project focuses on training a reinforcement learning (RL) agent to generate levels for a tile-based game. The agent learns to create game levels that satisfy specific constraints using Unity for the environment and RLlib for reinforcement learning.

## Project Overview

In this reinforcement learning task, the agent is trained to design game levels based on a set of predefined constraints. The goal is for the agent to learn how to place various items on a grid to create a playable and balanced game level.

### Constraints

The generated game levels must meet the following constraints:

- **Treasure Quantity**: The number of treasures must be between 5 and 15.
- **Items Quantity**: The number of items should be between 30 and 40.
- **Enemy Distance**: Each enemy must be positioned at least one tile away from the player.
- **Player Presence**: Exactly one player must be present in the level.
- **All Items**: All types of items must be present in the level.
- **Path to Treasures**: There must be a valid path from the player to all treasures.

## Project Structure

The project is divided into two main sections:

### 1. Environment (Unity)

The environment where the agent generates game levels is implemented in Unity. The Unity project can be found in the `Assets` folder. Key components include:

- **Tile Placement Logic**: The agent moves across each tile and either instantiates an object (e.g., treasure, item, enemy) or does nothing.
- **Restart Function**: When the agent reaches the end of the grid, the episode restarts.
- **Reward Function**: The agent receives rewards based on how closely the generated level meets the target constraints. The reward is calculated using `1 / (1 + abs(distance))`, where `distance` is the L1 norm of the difference between the current level configuration and the goal constraint vector.
- **Distance Calculator**: Computes the distance between the current vector and the goal vector.
- **Step Function**: Applies the policy action to the environment and updates the game level accordingly.
- **Collect Observations**: Gathers observations for the agent, including:
  - A grid array indicating which item is instantiated in each tile.
  - A vector representing the distance to the goal constraint vector.
- **Episode End Assessment**: Determines whether the episode has reached its conclusion.

### 2. Reinforcement Learning (RLlib)

The reinforcement learning component is implemented in RLlib and can be found in the `rllib_code` . This section includes:

- **Environment Connection**: The Unity environment is connected to RLlib in the `env` section.
- **Training**: The agent is trained using various settings, such as action and observation spaces, along with the PPO (Proximal Policy Optimization) algorithm settings.
- **Testing**: After training, the algorithm is tested in the environment to evaluate its performance.

## Building and Running the Project

### Unity Environment

To build the Unity environment, follow these steps:

1. Open the Unity project located in the `Assets` folder.
2. Build the project for your preferred platform (server-side or Windows).
3. Note the build path and configure the RLlib side to use this environment.

### RLlib Training

To train the agent using RLlib:

1. Navigate to the `rllib_code` folder.
2. Ensure that the Unity environment build is correctly referenced in the `env` section.
3. Run the training script with your preferred configuration.
4. Test the trained agent using the test script.

## Conclusion

This project demonstrates the application of reinforcement learning to generate game levels that meet specific constraints. By training the agent in a Unity environment with RLlib, the goal is to create balanced and playable levels for a tile-based game.

## Additional Information

For further details on the Unity environment setup or RLlib configuration, refer to the respective folders and documentation provided in the project.

