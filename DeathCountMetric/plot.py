# import gspread
# from oauth2client.service_account import ServiceAccountCredentials
# import pandas as pd
# from PIL import Image
# import matplotlib.pyplot as plt

# # -------------------------------
# # 1️⃣ Connect to Google Sheets
# # -------------------------------
# scope = ["https://spreadsheets.google.com/feeds", "https://www.googleapis.com/auth/drive"]
# creds = ServiceAccountCredentials.from_json_keyfile_name("credentials.json", scope)
# client = gspread.authorize(creds)

# # -------------------------------
# # 2️⃣ Open sheets
# # -------------------------------
# sheet_name = "Death"  # your Google Sheet name
# deaths_ws = client.open(sheet_name).worksheet("Deaths")
# map_ws = client.open(sheet_name).worksheet("Coordinate_Map")

# # -------------------------------
# # 3️⃣ Load data
# # -------------------------------
# deaths_df = pd.DataFrame(deaths_ws.get_all_records())
# map_df = pd.DataFrame(map_ws.get_all_records())

# # -------------------------------
# # 4️⃣ Conversion: world → pixel
# # -------------------------------
# def world_to_pixel(level_name, world_x, world_y):
#     level_name_clean = level_name.strip().lower()
#     map_df['Level_clean'] = map_df['Level'].str.strip().str.lower()
#     match = map_df[map_df['Level_clean'] == level_name_clean]

#     if match.empty:
#         print(f"⚠️ No mapping found for level: '{level_name}'")
#         return (None, None)

#     row = match.iloc[0]
#     world_min_x = row['World Min X']
#     world_max_x = row['World Max X']
#     world_min_y = row['World Min Y']
#     world_max_y = row['World Max Y']
#     image_width = row['Image Width']
#     image_height = row['Image Height']

#     # ✅ Offset adjustments (tweak these if dots look shifted)
#     offset_x = 80   # move right (+) or left (-)
#     offset_y = 20   # move up (+) or down (-)

#     pixel_x = (world_x - world_min_x) / (world_max_x - world_min_x) * image_width + offset_x
#     pixel_y = image_height - (world_y - world_min_y) / (world_max_y - world_min_y) * image_height + offset_y

#     return pixel_x, pixel_y

# # -------------------------------
# # 5️⃣ Filter & Convert (Level 1 only)
# # -------------------------------
# level_name = "flip level 1 New"  # must match your sheet entry
# level_df = deaths_df[deaths_df["Game Level"].str.lower().str.strip() == level_name.lower().strip()]

# pixels = level_df.apply(lambda row: world_to_pixel(row["Game Level"], row["Death Position X"], row["Death Position Y"]), axis=1)
# level_df["PixelX"] = [p[0] for p in pixels]
# level_df["PixelY"] = [p[1] for p in pixels]
# level_df = level_df.dropna(subset=["PixelX", "PixelY"])

# # -------------------------------
# # 6️⃣ Load level image
# # -------------------------------
# level_image_path = "images/Level1.png"  # update if in another folder
# img = Image.open(level_image_path)

# # -------------------------------
# # 7️⃣ Plot deaths
# # -------------------------------
# plt.figure(figsize=(img.width / 100, img.height / 100))
# plt.imshow(img)
# plt.scatter(level_df["PixelX"], level_df["PixelY"], c="red", s=30, alpha=0.6)
# plt.axis("off")
# plt.title("Deaths — Level 1", fontsize=18)
# plt.show()

# # -------------------------------
# # 8️⃣ (Optional) Save
# # -------------------------------
# plt.savefig("level1_with_deaths.png", bbox_inches="tight", pad_inches=0)


import gspread
from oauth2client.service_account import ServiceAccountCredentials
import pandas as pd
from PIL import Image
import matplotlib.pyplot as plt
import os

# -------------------------------
# 1️⃣ Connect to Google Sheets
# -------------------------------
scope = ["https://spreadsheets.google.com/feeds", "https://www.googleapis.com/auth/drive"]
creds = ServiceAccountCredentials.from_json_keyfile_name("credentials.json", scope)
client = gspread.authorize(creds)

# -------------------------------
# 2️⃣ Open sheets
# -------------------------------
sheet_name = "Death"
deaths_ws = client.open(sheet_name).worksheet("Deaths")
map_ws = client.open(sheet_name).worksheet("Coordinate_Map")

deaths_df = pd.DataFrame(deaths_ws.get_all_records())
map_df = pd.DataFrame(map_ws.get_all_records())

# -------------------------------
# 3️⃣ Conversion: world → pixel
# -------------------------------
# -------------------------------
# 3️⃣ Conversion: world → pixel (with per-level offsets)
# -------------------------------
def world_to_pixel(level_name, world_x, world_y):
    level_name_clean = level_name.strip().lower()
    map_df['Level_clean'] = map_df['Level'].str.strip().str.lower()
    match = map_df[map_df['Level_clean'] == level_name_clean]

    if match.empty:
        print(f"⚠️ No mapping found for level: '{level_name}'")
        return (None, None)

    row = match.iloc[0]
    world_min_x = row['World Min X']
    world_max_x = row['World Max X']
    world_min_y = row['World Min Y']
    world_max_y = row['World Max Y']
    image_width = row['Image Width']
    image_height = row['Image Height']

    # ⚙️ Per-level offsets
    level_offsets = {
        "flip level 1 new": (80, 20),       # move right (+) or left (-). # move up (+) or down (-)
        "flip level 2 update": (0, 0),  # tweak as needed
        "flip level 3 new": (-15, 5)      # tweak as needed
    }
    offset_x, offset_y = level_offsets.get(level_name_clean, (0, 0))

    # Convert to pixel coords
    pixel_x = (world_x - world_min_x) / (world_max_x - world_min_x) * image_width + offset_x
    pixel_y = image_height - (world_y - world_min_y) / (world_max_y - world_min_y) * image_height + offset_y
    return pixel_x, pixel_y

# -------------------------------
# 4️⃣ Level name → image name map
# -------------------------------
image_map = {
    "flip level 1 new": "images/Level1.png",
    "flip level 2 update": "images/Level2.png",
    "flip level 3 new": "images/Level3.png"
}

# -------------------------------
# 5️⃣ Loop through all levels
# -------------------------------
os.makedirs("outputs", exist_ok=True)

for level_name, image_path in image_map.items():
    print(f"📊 Plotting {level_name}...")

    # Find all death points for this level
    level_df = deaths_df[deaths_df["Game Level"].str.lower().str.strip() == level_name.lower().strip()]
    if level_df.empty:
        print(f"⚠️ No deaths recorded for {level_name}")
        continue

    # Convert to pixel coordinates
    pixels = level_df.apply(lambda row: world_to_pixel(row["Game Level"], row["Death Position X"], row["Death Position Y"]), axis=1)
    level_df["PixelX"] = [p[0] for p in pixels]
    level_df["PixelY"] = [p[1] for p in pixels]
    level_df = level_df.dropna(subset=["PixelX", "PixelY"])

    # Load corresponding image
    if not os.path.exists(image_path):
        print(f"⚠️ Image not found for {level_name}: {image_path}")
        continue

    img = Image.open(image_path)

    # Plot
    plt.figure(figsize=(img.width / 100, img.height / 100))
    plt.imshow(img)
    plt.scatter(level_df["PixelX"], level_df["PixelY"], c="red", s=30, alpha=0.6)
    plt.axis("off")
    plt.title(f"Deaths — {level_name}", fontsize=18)
    plt.tight_layout()

    # Save
    save_path = f"outputs/{level_name.replace(' ', '_')}_with_deaths.png"
    plt.savefig(save_path, bbox_inches="tight", pad_inches=0)
    plt.close()
    print(f"✅ Saved: {save_path}")

print("🎉 All levels processed!")
