package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/27/2014.
 */
public class BrowseButton extends SphericalButton {
    public BrowseButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int x = button.centerX();
        int y = button.top + padding;
        int halfWidth = button.width() >> 1;
        int left = x - (halfWidth - padding);
        int right = x + (button.width() - (padding << 2));

        shape.moveTo(x, y);
        shape.lineTo(left, button.centerY());
        shape.lineTo(right, button.centerY());
        shape.lineTo(x, y);

        y = button.centerY() + (strokeWidth << 1);
        shape.moveTo(left, y);
        shape.lineTo(right, y);
    }

    @Override
    public String command() {
        return PlayerCommand.BROWSE_CMD;
    }
}
